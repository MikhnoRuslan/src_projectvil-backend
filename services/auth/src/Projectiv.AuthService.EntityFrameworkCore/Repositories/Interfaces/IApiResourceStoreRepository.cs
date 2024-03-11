using Projectiv.AuthService.EntityFrameworkCore.Repositories.Interfaces;
using Projectiv.IdentityService.Domain.Models.Auth;

namespace Projectiv.AuthService.Domain.Interfaces;

public interface IApiResourceStoreRepository : IProjectivAuthBaseRepository<ApiResource>
{
    
}