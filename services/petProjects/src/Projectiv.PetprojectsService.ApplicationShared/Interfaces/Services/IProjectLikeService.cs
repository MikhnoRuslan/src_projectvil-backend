using Projectiv.PetprojectsService.ApplicationShared.Dtos.Project;

namespace Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;

public interface IProjectLikeService
{
    Task<List<ProjectLikeDto>> GetListAsync(CancellationToken cancellationToken = default);
    Task<ProjectLikeDto> LikeAsync(Guid projectId, CancellationToken cancellationToken = default);
}