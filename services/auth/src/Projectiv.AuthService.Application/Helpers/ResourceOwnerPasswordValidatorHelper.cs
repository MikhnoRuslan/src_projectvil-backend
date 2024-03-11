using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Projectiv.AuthService.DomainShared;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.AuthService.Application.Helpers;

public class ResourceOwnerPasswordValidatorHelper : IResourceOwnerPasswordValidator, ITransientDependence
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer _l;

    public ResourceOwnerPasswordValidatorHelper(UserManager<ApplicationUser> userManager, 
        IStringLocalizer l)
    {
        _userManager = userManager;
        _l = l;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var email = context.UserName;
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null && await _userManager.CheckPasswordAsync(user, context.Password))
        {
            if (!user.EmailConfirmed)
            {
                var customData = new Dictionary<string, object>
                {
                    { "user_id", user.Id },
                    { "email", user.Email }
                };
                
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, ProjectivWebGatewayErrorCode.EmailNotConfirmed, customData);
                return;
            }

            context.Result = new GrantValidationResult(
                subject: user.Id.ToString(),
                authenticationMethod: "pwd",
                claims: GetUserClaims(user));

            return;
        }

        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, _l[ProjectivWebGatewayErrorCode.InvalidUserNameOrPassword]);
    }
    
    private static IEnumerable<Claim> GetUserClaims(ApplicationUser user)
    {
        return new[]
        {
            new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
            new Claim(JwtClaimTypes.PreferredUserName, user.Email),
        };
    }
}