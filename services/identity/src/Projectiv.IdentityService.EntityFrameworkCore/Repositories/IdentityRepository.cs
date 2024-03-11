using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectiv.IdentityService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.IdentityService.EntityFrameworkCore.Repositories;

public class IdentityRepository : IdentityBaseRepository<ApplicationUser>, IIdentityRepository, ITransientDependence
{
    public IdentityRepository(ProjectivIdentityDbContext context) : base(context)
    {
    }
}