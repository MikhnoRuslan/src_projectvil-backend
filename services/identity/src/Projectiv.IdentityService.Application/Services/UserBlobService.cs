using Microsoft.AspNetCore.Http;
using Projectiv.IdentityService.ApplicationShared.Interfaces.Services;
using Projectiv.IdentityService.Domain.Models.BlobContainers;
using Projectvil.Shared.EntityFramework.Blob.Interfaces;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.IdentityService.Application.Services;

public class UserBlobService : IUserBlobService, ITransientDependence
{
    private readonly IBlobContainer<UserBlobContainer> _blob;

    public UserBlobService(IBlobContainer<UserBlobContainer> blob)
    {
        _blob = blob;
    }
    
    public async Task<Guid> CreateAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        return await _blob.CreateBlobAsync(file, cancellationToken: cancellationToken);
    }

    public async Task<IFormFile> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _blob.GetAsync(id, cancellationToken);
    }
}