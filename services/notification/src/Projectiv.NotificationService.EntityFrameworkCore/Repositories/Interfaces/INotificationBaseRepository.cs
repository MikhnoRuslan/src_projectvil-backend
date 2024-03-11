using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectiv.NotificationService.EntityFrameworkCore.Repositories.Interfaces.BaseRepository;

public interface INotificationBaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity<Guid>
{
    
}