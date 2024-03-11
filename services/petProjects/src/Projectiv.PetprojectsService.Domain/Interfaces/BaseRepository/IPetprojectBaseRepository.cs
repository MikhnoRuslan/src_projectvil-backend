using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectiv.PetprojectsService.Domain.Interfaces.BaseRepository;

public interface IPetprojectBaseRepository<TEntity> : IBaseRepository<TEntity> 
    where TEntity : class, IEntity<Guid>
{
    
}