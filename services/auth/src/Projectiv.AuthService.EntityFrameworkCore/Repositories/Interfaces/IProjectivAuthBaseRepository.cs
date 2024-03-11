using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectiv.AuthService.EntityFrameworkCore.Repositories.Interfaces;

public interface IProjectivAuthBaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class, IEntity<Guid>
{
    
}