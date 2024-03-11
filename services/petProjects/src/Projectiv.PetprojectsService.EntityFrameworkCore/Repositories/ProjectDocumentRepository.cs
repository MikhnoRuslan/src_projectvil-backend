using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.BaseRepository;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Repositories;

public class ProjectDocumentRepository : PetprojectBaseRepository<ProjectDocument>, IProjectDocumentRepository, ITransientDependence
{
    public ProjectDocumentRepository(PetProjectsDbContext context) : base(context)
    {
    }
}