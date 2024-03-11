using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Projectvil.Shared.EntityFramework.Blob.Interfaces;

public interface IBlobRepository
{
    Task<Models.Blob> GetAsync(Expression<Func<Models.Blob, bool>> predicate,
        CancellationToken cancellationToken = default);
    Task<IFormFile> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateBlobAsync(IFormFile file, CancellationToken cancellationToken = default);

    Task<Guid> UpdateAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}