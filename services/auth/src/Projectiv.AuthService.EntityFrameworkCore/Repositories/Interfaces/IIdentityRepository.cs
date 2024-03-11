using Projectiv.AuthService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.AuthService.Domain.Interfaces;

public interface IIdentityRepository : IProjectivAuthBaseRepository<ApplicationUser>
{
    
}