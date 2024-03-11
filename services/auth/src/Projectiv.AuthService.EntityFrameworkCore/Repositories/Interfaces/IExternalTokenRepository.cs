using Projectiv.AuthService.EntityFrameworkCore.Repositories.Interfaces;
using Projectiv.IdentityService.Domain.Models;

namespace Projectiv.AuthService.Domain.Interfaces;

public interface IExternalTokenRepository : IProjectivAuthBaseRepository<ExternalToken>
{
    Task<bool> RemoveByCodeAsync(string code);
}