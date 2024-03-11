using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectiv.IdentityService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Repositories;

namespace Projectiv.IdentityService.EntityFrameworkCore.Repositories;

public class IdentityBaseRepository<TEntity> : BaseRepository<TEntity, ProjectivIdentityDbContext>, IIdentityBaseRepository<TEntity>
    where TEntity : class, IEntity<Guid>
{
    public IdentityBaseRepository(ProjectivIdentityDbContext context) : base(context)
    {
    }
}