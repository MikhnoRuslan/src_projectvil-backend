using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.Application.Services.PetProjectService;

public class ProjectDocumentService : IProjectDocumentService, ITransientDependence
{
    private readonly IProjectDocumentRepository _projectDocumentRepository;

    public ProjectDocumentService(IProjectDocumentRepository productDocumentRepository)
    {
        _projectDocumentRepository = productDocumentRepository;
    }

    public async Task CreateManyAsync(List<Guid> documentsIds, Guid projectId,
        CancellationToken cancellationToken = default)
    {
        var models = documentsIds.Select(documentsId => new ProjectDocument(Guid.NewGuid())
        {
            ProjectId = projectId, 
            DocumentId = documentsId,
        }).ToList();

        await _projectDocumentRepository.InsertManyAsync(models, cancellationToken: cancellationToken);
    }

    public async Task<List<Guid>> CreateOrDeleteAsync(List<Guid> documentsIds, Guid projectId, CancellationToken cancellationToken)
    {
        var currentModels =
            await _projectDocumentRepository.GetListAsync(x => x.ProjectId == projectId, cancellationToken);
        
        var createModelsIds = documentsIds.Except(currentModels.Select(model => model.Id)).ToList();
        var deleteModelsIds = currentModels.Select(model => model.Id).Except(documentsIds).ToList();

        var createModels = createModelsIds.Select(documentId => new ProjectDocument(Guid.NewGuid())
        {
            ProjectId = projectId,
            DocumentId = documentId,
        }).ToList();

        var deleteModels = currentModels.Where(x => deleteModelsIds.Contains(x.Id)).ToList();

        if (createModels.Any())
            await _projectDocumentRepository.InsertManyAsync(createModels, cancellationToken: cancellationToken);
        
        if (deleteModels.Any())
            await _projectDocumentRepository.DeleteManyAsync(deleteModels, cancellationToken: cancellationToken);
        
        currentModels = await _projectDocumentRepository.GetListAsync(x => x.ProjectId == projectId, cancellationToken);

        return currentModels.Select(x => x.DocumentId).ToList();
    }

    public async Task<List<Guid>> GetDocumentsManyAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var models =
            await _projectDocumentRepository.GetListAsync(x => x.ProjectId == projectId,
                cancellationToken: cancellationToken);

        return models.Select(x => x.DocumentId).ToList();
    }
}