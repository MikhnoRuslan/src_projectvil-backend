namespace Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;

public interface IProjectDocumentService
{
    Task CreateManyAsync(List<Guid> documentsIds, Guid projectId,
        CancellationToken cancellationToken = default);

    Task<List<Guid>> GetDocumentsManyAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<List<Guid>> CreateOrDeleteAsync(List<Guid> documentsIds, Guid projectId, CancellationToken cancellationToken);
}