using IdentityServer4.Models;
using IdentityServer4.Stores;
using Newtonsoft.Json;
using Projectiv.AuthService.DomainShared.Configurations;
using Projectvil.Shared.Infrastructures.DI.Interfaces;
using IClientRepository = Projectiv.AuthService.Domain.Interfaces.IClientRepository;

namespace Projectiv.AuthService.Application.Helpers;

public class ClientStoreHelper : IClientStore, ITransientDependence
{
    private readonly IClientRepository _clientRepository;
    private readonly ProjectivAuthConfiguration _config;

    public ClientStoreHelper(IClientRepository clientRepository, 
        ProjectivAuthConfiguration config)
    {
        _clientRepository = clientRepository;
        _config = config;
    }

    public async Task<Client> FindClientByIdAsync(string clientId)
    {
        var client = await _clientRepository.GetAsync(x => x.ClientId.Equals(clientId));

        return new Client
        {
            ClientId = client.ClientId,
            Enabled = true,
            RequireConsent  = false,
            AllowOfflineAccess = true,
            RequirePkce = false,
            AllowedGrantTypes = new List<string>()
            {
                GrantType.ResourceOwnerPassword,
                GrantType.AuthorizationCode,
                GrantType.ClientCredentials
            },
            ClientName = client.ClientName,
            ClientSecrets = new List<Secret> { new(client.ClientSecrets) },
            AllowedScopes = JsonConvert.DeserializeObject<List<string>>(client.Scopes),
            RedirectUris = JsonConvert.DeserializeObject<List<string>>(client.RedirectUris),
            PostLogoutRedirectUris = JsonConvert.DeserializeObject<List<string>>(client.PostLogoutRedirectUris),
            AllowedCorsOrigins = _config.App.CorsOrigins
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s)
                .ToList()
        };
    }
}