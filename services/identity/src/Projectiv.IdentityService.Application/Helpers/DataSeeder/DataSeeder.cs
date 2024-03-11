using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Projectiv.IdentityService.ApplicationShared.Interfaces;
using Projectiv.IdentityService.DomainShared.Configuration;
using Projectiv.IdentityService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures.Constants;
using Projectvil.Shared.Infrastructures.DI.Interfaces;
using ApiResource = Projectiv.IdentityService.Domain.Models.Auth.ApiResource;
using Client = Projectiv.IdentityService.Domain.Models.Auth.Client;

namespace Projectiv.IdentityService.Application.Helpers.DataSeeder;

public class DataSeeder : IDataSeeder, ITransientDependence
{
    private readonly IClientRepository _clientRepository;
    private readonly IApiResourceStoreRepository _apiResourceStoreRepository;
    private readonly IdentityConfiguration _config;
    private readonly ILogger<DataSeeder> _logger;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityResourceRepository _identityResourceRepository;

    public DataSeeder(IClientRepository clientRepository,
        IApiResourceStoreRepository apiResourceStoreRepository,
        IdentityConfiguration config,
        ILogger<DataSeeder> logger, 
        RoleManager<IdentityRole<Guid>> roleManager, 
        UserManager<ApplicationUser> userManager, 
        IIdentityResourceRepository identityResourceRepository)
    {
        _clientRepository = clientRepository;
        _apiResourceStoreRepository = apiResourceStoreRepository;
        _config = config;
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
        _identityResourceRepository = identityResourceRepository;
    }

    public async Task SeedAsync()
    {
        await SeedAuthConfigAsync();
        await SeedIdentityAsync();
    }

    private async Task SeedAuthConfigAsync()
    {
        var clientNames = GetClientNames();
        var apiNames = GetApiNames();
        var resources = GetIdentityResources();

        var lstClients = await _clientRepository.GetListAsync();
        var lstApiResources = await _apiResourceStoreRepository.GetListAsync();
        var lstResources = await _identityResourceRepository.GetListAsync();

        var clients = clientNames.Select(clientName => new Client(Guid.NewGuid())
            {
                ClientId = clientName.Key,
                ClientName = clientName.Value,
                ClientSecrets = HashExtensions.Sha256(_config.AuthServer.Secret),
                Scopes = JsonConvert.SerializeObject(apiNames),
                RedirectUris = GetRedirectUris(clientName.Key),
                PostLogoutRedirectUris = GetRedirectUris(clientName.Key)
            })
            .Where(client => lstClients.FirstOrDefault(x => x.ClientId == client.ClientId) == null)
            .ToList();

        var apiResources = apiNames
            .Select(apiName => new ApiResource(Guid.NewGuid())
                { 
                    Name = apiName, 
                    Scopes = JsonConvert.SerializeObject(new List<string> { apiName }) 
                })
            .Where(apiResource => lstApiResources.FirstOrDefault(x => x.Name == apiResource.Name) == null).ToList();

        var identityResources = resources.Select(resource =>
                new Projectiv.IdentityService.Domain.Models.Auth.IdentityResource(Guid.NewGuid())
                {
                    Name = resource.Name,
                    DisplayName = resource.DisplayName,
                    Claims = JsonConvert.SerializeObject(resource.UserClaims)
                })
            .Where(identityResource => lstResources.FirstOrDefault(x => x.Name == identityResource.Name) == null)
            .ToList();

        if (clients.Count > 0)
            await _clientRepository.InsertManyAsync(clients, true);

        if (apiResources.Count > 0)
            await _apiResourceStoreRepository.InsertManyAsync(apiResources, true);
        
        if (identityResources.Count > 0)
            await _identityResourceRepository.InsertManyAsync(identityResources, true);
    }

    private async Task SeedIdentityAsync()
    {
        _logger.LogWarning("Start seed");

        if (await _roleManager.FindByNameAsync("Admin") == null)
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
        }

        if (_userManager.FindByEmailAsync("user@gmail.com").Result == null) 
        {
            //USER
            var bob = new ApplicationUser()
            {
                UserName = "Bob James",
                Email = "user@gmail.com",
                EmailConfirmed = true,
            };

            var userResult = await _userManager.CreateAsync(bob, "User123*");
            if (userResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(bob, "User");
                await _userManager.AddClaimsAsync(bob, new Claim[]
                    {
                    new Claim(JwtClaimTypes.Name, "Bob James"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "James"),
                    new Claim(JwtClaimTypes.Role, "User"),
                    });
            }
        }

        if (_userManager.FindByEmailAsync("admin@gmail.com").Result == null) 
        {
            //ADMIN
            var admin = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            var adminResult = await _userManager.CreateAsync(admin, "Admin123*");
            if (adminResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
                await _userManager.AddClaimsAsync(admin, new Claim[]
                {
                new Claim(JwtClaimTypes.Name, admin.UserName),
                new Claim(JwtClaimTypes.GivenName, "adminFirst"),
                new Claim(JwtClaimTypes.FamilyName, "adminLast"),
                new Claim(JwtClaimTypes.Role, "Admin"),
                });
            }
        }

        _logger.LogWarning("End seed");
    }
    
    private Dictionary<string, string> GetClientNames()
    {
        return new Dictionary<string, string>()
        {
            { "swagger-client-id", "Swagger UI" },
            { "react-client-id", "Projectiv React App" }
        };
    }

    private List<string> GetApiNames()
    {
        return new List<string>()
        {
            "openid",
            "profile",
            "password",
            ProjectivConstants.Api.PetProjectApi,
            ProjectivConstants.Api.IdentityApi,
            ProjectivConstants.Api.AuthApi,
            ProjectivConstants.Api.GetWayApi,
            ProjectivConstants.Api.NotificationApi
        };
    }

    private List<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }

    private string GetRedirectUris(string clientId)
    {
        const string postfix = "/swagger/oauth2-redirect.html";
        
        return clientId.Equals("swagger-client-id")
            ? JsonConvert.SerializeObject(new List<string>
            {
                _config.OpenIddict.Applications.AuthServer + postfix,
                _config.OpenIddict.Applications.IdentityService + postfix,
                _config.OpenIddict.Applications.PetProjectService + postfix,
                _config.OpenIddict.Applications.GateWayService + postfix,
                _config.OpenIddict.Applications.NotificationService + postfix,
            })
            : JsonConvert.SerializeObject(new List<string> { _config.AuthServer.RedirectUrl });
    }
}