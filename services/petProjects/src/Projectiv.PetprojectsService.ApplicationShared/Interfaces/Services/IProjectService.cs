using Projectiv.PetprojectsService.ApplicationShared.Dtos.Project;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectvil.Shared.EntityFramework.Models.FilterModels;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;

public interface IProjectService
{
    Task<ProjectDto> GetAsync(Guid id, ELanguage language, CancellationToken cancellationToken = default);
    
    Task<PageResultDto<ProjectDto>> GetListAsync(ProjectListInput input, ELanguage language,
        CancellationToken cancellationToken = default);
    
    Task<ProjectDto> CreateAsync(CreateProjectInput input, ELanguage language,
        CancellationToken cancellationToken = default);

    Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectInput input, ELanguage language,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}