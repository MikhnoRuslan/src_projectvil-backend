/*using IdentityServer4.Services;
using IdentityServer4.Validation;
using Projectiv.AuthService.Application.Helpers;
using Projectiv.AuthService.Domain.Interfaces;
using Projectiv.AuthService.EntityFrameworkCore.Repositories;
using Projectvil.Shared.EntityFramework.Helpers;
using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectiv.AuthService.Host;

public static class Dependencies
{
    public static void AddDependencies(this IServiceCollection services)
    {
        // repos
        services.AddHttpContextAccessor();
        services.AddTransient<IApiResourceStoreRepository, ApiResourceStoreRepository>();
        services.AddTransient<IIdentityResourceStoreRepository, IdentityResourceStoreRepository>();
        services.AddTransient<IClientRepository, ClientRepository>();
        services.AddTransient<IProfileService, ProjectivProfileHelper>();
        services.AddTransient<IExternalTokenRepository, ExternalTokenRepository>();
        services.AddTransient<IIdentityRepository, IdentityRepository>();
        services.AddScoped<ICurrentUserHelper, CurrentUserHelper>();
        
        // helpers
        services.AddTransient<ClientStoreHelper>();
        services.AddTransient<CustomResourceStoreHelper>();
        services.AddTransient<IEntityTimestampUpdater, EntityTimestampUpdater>();
        
        // validators
        services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidatorHelper>();
        services.AddTransient<IExtensionGrantValidator, GoogleGrantValidatorHelper>();
    }
}*/