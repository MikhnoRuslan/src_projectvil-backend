using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Projectvil.Shared.EntityFramework.Blob.Models;

namespace Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;

public interface IPetProjectBlobService
{
    Task<Blob> GetAsync(Expression<Func<Blob, bool>> predicate,
        CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task<IFormFile> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> UpdateAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}