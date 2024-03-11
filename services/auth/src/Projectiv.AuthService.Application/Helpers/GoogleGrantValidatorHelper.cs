using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Projectiv.AuthService.ApplicationShared.Dtos;
using Projectiv.AuthService.Domain.Interfaces;
using Projectiv.AuthService.DomainShared;
using Projectiv.IdentityService.Domain.Models;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.AuthService.Application.Helpers;

public class GoogleGrantValidatorHelper : IExtensionGrantValidator, ITransientDependence
{
    private const string ProviderName = "Google";
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IExternalTokenRepository _externalTokenRepository;

    public GoogleGrantValidatorHelper(UserManager<ApplicationUser> userManager, 
        IConfiguration configuration, 
        IExternalTokenRepository externalTokenRepository)
    {
        _userManager = userManager;
        _configuration = configuration;
        _externalTokenRepository = externalTokenRepository;
    }
    
    public string GrantType => "google_auth";

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var code = context.Request.Raw.Get("code");
        var redirectUrl = context.Request.Raw.Get("return_url");
        var localEmail = context.Request.Raw.Get("localEmail");
        var localPassword = context.Request.Raw.Get("localPassword");
        var email = context.Request.Raw.Get("email");
        var clientId = _configuration.GetValue<string>("Google:ClientID");
        var clientSecret = _configuration.GetValue<string>("Google:ClientSecret");
        
        TokensMappingDto tokens = new();
        
        if (string.IsNullOrEmpty(code))
        {
            //You may want to add some claims here.
            context.Result = new GrantValidationResult(OidcConstants.TokenErrors.InvalidGrant, null);
            return;
        }
        //Validate code 
        //https://accounts.google.com/o/oauth2/token
        HttpClient client = new HttpClient();
        var dbAccessToken = await GetAccessTokenByCode(code);
        if (dbAccessToken == null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUrl },
                { "grant_type", "authorization_code" }
            };

            var content = new FormUrlEncodedContent(parameters);
            var tokensRequest =
                client.PostAsync($"https://accounts.google.com/o/oauth2/token", content)
                    .Result; //тут получаем access_token

            if (tokensRequest.StatusCode == System.Net.HttpStatusCode.OK)//если запрос 200
            {
                tokens = await MapTokensAsync(tokensRequest);
                if (!tokens.Succeeded)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "mapping_token_error");
                    return;
                }

                var tokenSavingResult = await SaveAccessTokenAsync(code, tokens);
                if (!tokenSavingResult)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "saving_token_error");
                    return;
                }
            }
            else // иначе ошибка
            {
                context.Result =
                    new GrantValidationResult(TokenRequestErrors.InvalidGrant, "external_provider_request_error");
                return;
            }
        }
        else//если в базе есть
        {
            if (IsAccessTokenExpired(new ExternalToken()//не expired ли
                {
                    AccessTokenExpirationSeconds = dbAccessToken.ExpiresIn,
                    AccessTokenDateCreated = dbAccessToken.AccessTokenDateCreated
                }))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "access_token_expired");
                return;
            }

            tokens = dbAccessToken;
        }

        var request = client.GetAsync($"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={tokens.AccessToken}").Result;

        if (request.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var userMappingResult = await MapUser(request);
            if (!userMappingResult.Succeeded)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "mapping_error");
                return;
            }

            var findExternalLoginResult = await TryToFindUser(userMappingResult.Id);
            if (findExternalLoginResult != null)
            {
                await _externalTokenRepository.RemoveByCodeAsync(code); //при каждой удачной попытке логина удаляем токен из базы
                context.Result = new GrantValidationResult(findExternalLoginResult.Id.ToString(), "google");
                return;
            }
            
            var newUser = new ApplicationUser()
            {
                FirstName = userMappingResult.FirstName,
                LastName = userMappingResult.LastName,
                Email = userMappingResult.Email ?? email,
                EmailConfirmed = userMappingResult.Email == null ? false : true,
                UserName = Guid.NewGuid().ToString(),
                Country = userMappingResult.Country
            };
            
            if (newUser.Email == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "specify_email");
                return;
            }
            
            var isLocalAccountExist = await IsLocalAccountExist(userMappingResult.Email ?? email);
            if (isLocalAccountExist)
            {
                if (localEmail != null && localPassword != null)
                {
                    var result = await MergeLocalAndExternal(localEmail, localPassword, userMappingResult.Id, userMappingResult.Email);
                    if (result.Succeeded)
                    {
                        await _externalTokenRepository.RemoveByCodeAsync(code);
                        context.Result = new GrantValidationResult(result.UserId, "google");
                        return;
                    }
                    else
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, result.ErrorText);
                        return;
                    }
                }

                Dictionary<string, object> customResp = new();
                customResp.Add("Email",userMappingResult.Email ?? email);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "local_account_exist",customResp);
                return;
            }
            
            var creatingResult = await CreateNewUser(newUser, userMappingResult.Id);

            if (creatingResult)
            {
                await _externalTokenRepository.RemoveByCodeAsync(code);
                context.Result = new GrantValidationResult(newUser.Id.ToString(), "google");
                return;
            }
            
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,"error_while_creating_new_user");
            return;

        }
        
        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,"external_provider_request_error");
        return;
    }
    
    private async Task<bool> CreateNewUser(ApplicationUser user, string providerUserId)
    {
        var creatingResult = await _userManager.CreateAsync(user);
        if (creatingResult.Succeeded)
        {
            //todo
            await _userManager.AddToRoleAsync(user, "User");
            await _userManager.AddLoginAsync(user, new UserLoginInfo(ProviderName, providerUserId, "Google"));

            return true;
        }

        return false;
    }
    
    private async Task<MergeLocalAndExternalDto> MergeLocalAndExternal(string localEmail, string localPassword, 
        string providerUserId, string externalEmail)
    {
        var user = await _userManager.FindByEmailAsync(localEmail);
        if (user == null)
        {
            return new MergeLocalAndExternalDto()
            {
                ErrorText = ProjectivWebGatewayErrorCode.WrongLocalEmail,
                Succeeded = false
            };
        }

        if (!await _userManager.CheckPasswordAsync(user, localPassword))
        {
            //wrong password
            return new MergeLocalAndExternalDto()
            {
                ErrorText = ProjectivWebGatewayErrorCode.WrongLocalPassword,
                Succeeded = false
            };
        }
        
        if (user.Email == externalEmail && !user.EmailConfirmed)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmationResult.Succeeded)
            {
                return new MergeLocalAndExternalDto()
                {
                    ErrorText = ProjectivWebGatewayErrorCode.EmailConfirmError,
                    Succeeded = false
                };
            }
        }
        var result = await _userManager.AddLoginAsync(user, new UserLoginInfo(ProviderName, providerUserId, "Google"));
        if (result.Succeeded)
        {
            return new MergeLocalAndExternalDto()
            {
                UserId = user.Id.ToString(),
                Succeeded = true
            };
        }

        return new MergeLocalAndExternalDto()
        {
            ErrorText = ProjectivWebGatewayErrorCode.MergeError,
            Succeeded = false
        };
    }
    
    private async Task<ApplicationUser> TryToFindUser(string providerUserId)
    {
        return await _userManager.FindByLoginAsync(ProviderName, providerUserId);
    }
    
    private async Task<bool> SaveAccessTokenAsync(string code, TokensMappingDto accessToken)
    {
        var accessTokenDbModel = new ExternalToken()
        {
            AutorizationCode = code,
            AccessToken = accessToken.AccessToken,
            AccessTokenExpirationSeconds = accessToken.ExpiresIn,
            AccessTokenDateCreated = DateTime.UtcNow,
            ProviderUserId = accessToken.ProviderUserId,
            Provider = ProviderName
        };

        try
        {
            await _externalTokenRepository.InsertAsync(accessTokenDbModel);
        }
        catch
        {
            return false;
        }
        
        return true;
        
    }

    private async Task<TokensMappingDto> GetAccessTokenByCode(string code)
    {
        var token = await _externalTokenRepository.GetAsync(x => x.AutorizationCode.Equals(code));
        if (token == null) return null;
        
        var mappingToken = new TokensMappingDto()
        {
            AccessToken = token.AccessToken,
            ProviderUserId = token.ProviderUserId,
            ExpiresIn = token.AccessTokenExpirationSeconds,
            AccessTokenDateCreated = token.AccessTokenDateCreated
        };

        return mappingToken;
    }

    private bool IsAccessTokenExpired(ExternalToken accessTokenInfo)
    {
        var now = DateTime.UtcNow;

        return (now - accessTokenInfo.AccessTokenDateCreated).TotalSeconds > accessTokenInfo.AccessTokenExpirationSeconds;
    }
    
    private async Task<bool> IsLocalAccountExist(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }
    private async Task<TokensMappingDto> MapTokensAsync(HttpResponseMessage tokenOutput)
    {
        var json = await tokenOutput.Content.ReadAsStringAsync();
        var tokenInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        var idToken = tokenInfo["id_token"];
        var accessToken = tokenInfo["access_token"];
        var expiration = Int32.Parse(tokenInfo["expires_in"]);
        
        if (idToken == null)
        {
            return null;
        }

        var tokensResult = new TokensMappingDto()
        {
            IdToken = idToken,
            AccessToken = accessToken,
            ExpiresIn = expiration
        };

        return tokensResult;
    }
    
    private async Task<UserMappingDto> MapUser(HttpResponseMessage userInfoOutput)
    {
        var json = await userInfoOutput.Content.ReadAsStringAsync();
        var userInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        if (userInfo != null)
        {
            userInfo.TryGetValue("email", out var email);
            userInfo.TryGetValue("given_name", out var firstName);
            userInfo.TryGetValue("family_name", out var lastName);
            userInfo.TryGetValue("country", out var country);
            userInfo.TryGetValue("id", out var id);
            
            var model = new UserMappingDto()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Country = country,
                Email = email,
            };
            return model;
        }

        return new UserMappingDto()
        {
            Succeeded = false
        };
    }
}