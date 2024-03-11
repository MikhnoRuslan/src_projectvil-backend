using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.PetprojectsService.Domain.Models.BlobContainers;
using Projectvil.Shared.EntityFramework.Blob.Interfaces;
using Projectvil.Shared.EntityFramework.Blob.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.Application.Services.PetProjectService;

public class PetProjectBlobService : IPetProjectBlobService, ITransientDependence
{
    private readonly IBlobContainer<FirstBlobContainer> _blob;

    public PetProjectBlobService(IBlobContainer<FirstBlobContainer> blob)
    {
        _blob = blob;
    }
    
    public async Task<IFormFile> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _blob.GetAsync(id, cancellationToken);
    }

    public async Task<Blob> GetAsync(Expression<Func<Blob, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _blob.GetAsync(predicate, cancellationToken);
    }

    public async Task<Guid> CreateAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        return await _blob.CreateBlobAsync(file, cancellationToken: cancellationToken);
    }

    public async Task<Guid> UpdateAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default)
    {
        return await _blob.UpdateAsync(id, file, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _blob.DeleteAsync(id, cancellationToken);
    }
}