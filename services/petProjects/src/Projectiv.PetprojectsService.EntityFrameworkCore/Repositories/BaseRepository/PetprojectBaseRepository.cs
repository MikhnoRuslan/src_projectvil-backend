using Projectiv.PetprojectsService.Domain.Interfaces.BaseRepository;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Repositories;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.BaseRepository;

public class PetprojectBaseRepository<TEntity> : BaseRepository<TEntity, PetProjectsDbContext>, IPetprojectBaseRepository<TEntity>
    where TEntity : class, IEntity<Guid>
{
    public PetprojectBaseRepository(PetProjectsDbContext context) 
        : base(context)
    {
    }
}