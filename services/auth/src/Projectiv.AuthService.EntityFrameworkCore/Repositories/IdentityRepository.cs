using Projectiv.AuthService.Domain.Interfaces;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.AuthService.EntityFrameworkCore.Repositories;

public class IdentityRepository : ProjectivAuthBaseRepository<ApplicationUser>, IIdentityRepository, ITransientDependence
{
    public IdentityRepository(ProjectivIdentityDbContext context) : base(context)
    {
    }
    
    
}