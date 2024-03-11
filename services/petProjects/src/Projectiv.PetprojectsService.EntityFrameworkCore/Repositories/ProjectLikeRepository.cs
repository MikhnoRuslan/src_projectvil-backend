using Projectiv.PetprojectsService.Domain.Interfaces;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.BaseRepository;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Repositories;

public class ProjectLikeRepository : PetprojectBaseRepository<ProjectLike>, IProjectLikeRepository, ITransientDependence
{
    public ProjectLikeRepository(PetProjectsDbContext context) : base(context)
    {
    }
}