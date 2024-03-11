using Projectiv.AuthService.Domain.Interfaces;
using Projectiv.IdentityService.Domain.Models.Auth;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.AuthService.EntityFrameworkCore.Repositories;

public class IdentityResourceStoreRepository : ProjectivAuthBaseRepository<IdentityResource>, IIdentityResourceStoreRepository, ITransientDependence
{
    public IdentityResourceStoreRepository(ProjectivIdentityDbContext context) 
        : base(context)
    {
    }
}