using Projectiv.AuthService.Domain.Interfaces;
using Projectiv.AuthService.EntityFrameworkCore.Repositories.Interfaces;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Repositories;

namespace Projectiv.AuthService.EntityFrameworkCore.Repositories;

public class ProjectivAuthBaseRepository<TEntity> : BaseRepository<TEntity, ProjectivIdentityDbContext>, IProjectivAuthBaseRepository<TEntity>
    where TEntity : class, IEntity<Guid>
{
    public ProjectivAuthBaseRepository(ProjectivIdentityDbContext context) 
        : base(context)
    {
    }
}