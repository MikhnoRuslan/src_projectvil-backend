using Projectiv.AuthService.Domain.Interfaces;
using Projectiv.IdentityService.Domain.Models.Auth;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.AuthService.EntityFrameworkCore.Repositories;

public class ApiResourceStoreRepository : ProjectivAuthBaseRepository<ApiResource>, IApiResourceStoreRepository, ITransientDependence
{
    public ApiResourceStoreRepository(ProjectivIdentityDbContext context) 
        : base(context)
    {
    }
}