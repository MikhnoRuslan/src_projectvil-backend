using Projectiv.NotificationService.EntityFrameworkCore.Context;
using Projectiv.NotificationService.EntityFrameworkCore.Repositories.Interfaces.BaseRepository;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Repositories;

namespace Projectiv.NotificationService.EntityFrameworkCore.Repositories.BaseRepository;

public class NotificationBaseRepository<TEntity> : BaseRepository<TEntity, NotificationDbContext>, INotificationBaseRepository<TEntity>
    where TEntity : class, IEntity<Guid>
{
    public NotificationBaseRepository(NotificationDbContext context) 
        : base(context)
    {
    }
}