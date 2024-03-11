using IdentityServer4.Models;
using IdentityServer4.Stores;
using Newtonsoft.Json;
using Projectiv.AuthService.Domain.Interfaces;
using Projectvil.Shared.Infrastructures.DI.Interfaces;
using IApiResourceStoreRepository = Projectiv.AuthService.Domain.Interfaces.IApiResourceStoreRepository;

namespace Projectiv.AuthService.Application.Helpers;

public class CustomResourceStoreHelper : IResourceStore, ITransientDependence
{
    private readonly IApiResourceStoreRepository _apiResourceStoreRepository;
    private readonly IIdentityResourceStoreRepository _identityResourceStoreRepository;

    public CustomResourceStoreHelper(IApiResourceStoreRepository apiResourceStoreRepository, 
        IIdentityResourceStoreRepository identityResourceStoreRepository)
    {
        _apiResourceStoreRepository = apiResourceStoreRepository;
        _identityResourceStoreRepository = identityResourceStoreRepository;
    }

    public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        var resources = await _identityResourceStoreRepository.GetListAsync(r => scopeNames.Contains(r.Name));
        return resources?.Select(r => new IdentityResource(r.Name, r.DisplayName, JsonConvert.DeserializeObject<List<string>>(r.Claims))) ?? Enumerable.Empty<IdentityResource>();
    }

    public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        var allApiResources = await _apiResourceStoreRepository.GetListAsync();

        var filteredScopes = allApiResources
            .SelectMany(ar => {
                var scopes = JsonConvert.DeserializeObject<List<string>>(ar.Scopes);
                return scopes.Where(s => scopeNames.Contains(s.Trim()));
            })
            .Distinct()
            .Select(s => new ApiScope(s.Trim()));
        
        return filteredScopes;
    }

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        var allApiResources = await _apiResourceStoreRepository.GetListAsync();

        var filteredApiResources = allApiResources
            .Where(x => {
                var scopes = JsonConvert.DeserializeObject<List<string>>(x.Scopes);
                return scopeNames.Any(s => scopes.Contains(s));
            });

        return filteredApiResources.Select(ar => new ApiResource(ar.Name, JsonConvert.DeserializeObject<List<string>>(ar.Scopes)));
    }

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        var apiResources = await _apiResourceStoreRepository.GetListAsync(x => apiResourceNames.Contains(x.Name));
        return apiResources.Select(ar => new ApiResource(ar.Name, JsonConvert.DeserializeObject<List<string>>(ar.Scopes)));
    }

    public async Task<Resources> GetAllResourcesAsync()
    {
        var apiResources = await _apiResourceStoreRepository.GetListAsync();
        var identityResources = Enumerable.Empty<IdentityResource>();
        var apiResourceModels = apiResources.Select(ar => new ApiResource(ar.Name, JsonConvert.DeserializeObject<List<string>>(ar.Scopes)));
        var apiScopes = apiResources
            .SelectMany(ar => JsonConvert.DeserializeObject<List<string>>(ar.Scopes))
            .Distinct() 
            .Select(scope => new ApiScope(scope));
        
        return new Resources(identityResources, apiResourceModels, apiScopes);
    }
}