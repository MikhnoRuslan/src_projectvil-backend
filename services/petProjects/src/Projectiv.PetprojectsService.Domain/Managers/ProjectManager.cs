using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.Domain.Managers;

public class ProjectManager : IProjectManager, ITransientDependence
{
    private readonly IProjectDocumentRepository _productDocumentService;
    private readonly IProjectRepository _projectRepository;
    private readonly IPetProjectBlobService _blob;

    public ProjectManager(IProjectDocumentRepository productDocumentService,
        IProjectRepository projectRepository, 
        IPetProjectBlobService blob)
    {
        _productDocumentService = productDocumentService;
        _projectRepository = projectRepository;
        _blob = blob;
    }

    public async Task DeleteAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var documents = await _productDocumentService.GetListAsync(x => x.ProjectId == projectId, cancellationToken);
        var project = await _projectRepository.GetAsync(x => x.Id == projectId, cancellationToken);

        foreach (var document in documents)
        {
            await _blob.DeleteAsync(document.DocumentId, cancellationToken);
        }

        await _projectRepository.DeleteAsync(project, true, cancellationToken: cancellationToken);
    }
}

public interface IProjectManager
{
    Task DeleteAsync(Guid projectId, CancellationToken cancellationToken = default);
}