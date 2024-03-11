using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Projectvil.Shared.EntityFramework.Blob.Context;

public interface IBlobContextDatabase : IBlobContext
{
    Task<int> SaveAsync(CancellationToken cancellationToken = default);

    ValueTask<EntityEntry<TEntity>> AddEntityAsync<TEntity>(TEntity entity,
        CancellationToken cancellationToken = default) where TEntity : class;

    EntityEntry<TEntity> RemoveEntity<TEntity>(TEntity entity)
        where TEntity : class;
}