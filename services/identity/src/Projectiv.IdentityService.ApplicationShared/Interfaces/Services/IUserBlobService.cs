using Microsoft.AspNetCore.Http;

namespace Projectiv.IdentityService.ApplicationShared.Interfaces.Services;

public interface IUserBlobService
{
    Task<Guid> CreateAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task<IFormFile> GetAsync(Guid id, CancellationToken cancellationToken = default);
}