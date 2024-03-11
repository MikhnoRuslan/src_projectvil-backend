using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.IdentityService.EntityFrameworkCore.Repositories.Interfaces;

public interface IIdentityRepository : IIdentityBaseRepository<ApplicationUser>
{
    
}