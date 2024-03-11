using Projectiv.PetprojectsService.ApplicationShared.Dtos.Project;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.PetprojectsService.Domain.Interfaces;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.Application.Services.PetProjectService;

public class ProjectLikeService : IProjectLikeService, ITransientDependence
{
    private readonly IProjectLikeRepository _projectLikeRepository;
    private readonly ICurrentUserHelper _currentUser;

    public ProjectLikeService(IProjectLikeRepository projectLikeRepository,
        ICurrentUserHelper currentUser)
    {
        _projectLikeRepository = projectLikeRepository;
        _currentUser = currentUser;
    }

    public async Task<List<ProjectLikeDto>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var currentUser = _currentUser.GetCurrentUserInfo();
        var models = await _projectLikeRepository.GetListAsync(cancellationToken);

        var views = models.GroupBy(x => x.ProjectId)
            .Select(x => new ProjectLikeDto()
            {
                ProjectId = x.Key,
                Likes = x.Count(),
                IsLike = x.FirstOrDefault(t => t.UserId == currentUser.Id) != null
            }).ToList();

        return views;
    }

    public async Task<ProjectLikeDto> LikeAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var currentUser = _currentUser.GetCurrentUserInfo();

        var models = await _projectLikeRepository.GetListAsync(x => x.ProjectId == projectId, cancellationToken);

        if (models.Any(x => x.UserId == currentUser.Id))
        {
            var model = await _projectLikeRepository.GetAsync(
                x => x.ProjectId == projectId && x.UserId == currentUser.Id, cancellationToken);

            await _projectLikeRepository.DeleteAsync(model, cancellationToken: cancellationToken);
            
            return new ProjectLikeDto()
            {
                ProjectId = projectId,
                Likes = models.Count() - 1,
                IsLike = false
            };
        }

        var like = new ProjectLike(Guid.NewGuid())
        {
            ProjectId = projectId,
            UserId = currentUser.Id!.Value
        };

        await _projectLikeRepository.InsertAsync(like, cancellationToken: cancellationToken);
        
        return new ProjectLikeDto()
        {
            ProjectId = projectId,
            Likes = models.Count() + 1,
            IsLike = true
        };
    }
}