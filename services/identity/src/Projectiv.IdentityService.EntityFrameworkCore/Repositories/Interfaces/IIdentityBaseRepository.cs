using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectiv.IdentityService.EntityFrameworkCore.Repositories.Interfaces;

public interface IIdentityBaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class, IEntity<Guid>
{
    
}