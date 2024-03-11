using AutoMapper;
using Grpc.Core;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Project;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.ProjectServices.Grpc;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectiv.PetprojectsService.Api.Grpc;

public class ProjectGrpcService : ProjectPublic.ProjectPublicBase
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;
    private readonly IProjectLikeService _projectLikeService;

    public ProjectGrpcService(IProjectService projectService,
        IMapper mapper,
        IProjectLikeService projectLikeService)
    {
        _projectService = projectService;
        _mapper = mapper;
        _projectLikeService = projectLikeService;
    }

    public override async Task<ProjectResponse> Get(GetProjectRequest request, ServerCallContext context)
    {
        var result = await _projectService.GetAsync(Guid.Parse(request.Id), (ELanguage)request.Language,
            context.CancellationToken);

        return _mapper.Map<ProjectDto, ProjectResponse>(result);
    }

    public override async Task<PageResultProjectResponse> GetList(GetListProjectRequest request, ServerCallContext context)
    {
        var input = _mapper.Map<GetListProjectRequest, ProjectListInput>(request);
        var models = await _projectService.GetListAsync(input, (ELanguage)request.Language, context.CancellationToken);
        var likes = await _projectLikeService.GetListAsync(context.CancellationToken);
        
        var views = models.Items.Select(item => _mapper.Map<ProjectDto, ProjectResponse>(item)).ToList();

        foreach (var view in views)
        {
            var projectId = Guid.Parse(view.Id);
            var like = likes.FirstOrDefault(x => x.ProjectId == projectId) ?? new ProjectLikeDto()
            {
                ProjectId = projectId,
                Likes = 0,
                IsLike = false
            };

            view.Likes = _mapper.Map<ProjectLikeDto, LikeResponse>(like);
        }
        
        return new PageResultProjectResponse()
        {
            TotalCount = (int)models.TotalCount,
            Data = { views }
        };
    }

    public override async Task<ProjectResponse> Create(CreateProjectRequest request, ServerCallContext context)
    {
        var input = _mapper.Map<CreateProjectRequest, CreateProjectInput>(request);
        var result = await _projectService.CreateAsync(input, (ELanguage)request.Language, context.CancellationToken);
        
        return _mapper.Map<ProjectDto, ProjectResponse>(result);
    }

    public override async Task<ProjectResponse> Update(UpdateProjectRequest request, ServerCallContext context)
    {
        var input = _mapper.Map<UpdateProjectRequest, UpdateProjectInput>(request);
        var result = await _projectService.UpdateAsync(Guid.Parse(request.Id), input, (ELanguage)request.Language,
            context.CancellationToken);
        
        return _mapper.Map<ProjectDto, ProjectResponse>(result);
    }

    public override async Task<EmptyResponse> Delete(DeleteProjectRequest request, ServerCallContext context)
    {
        await _projectService.DeleteAsync(Guid.Parse(request.Id), context.CancellationToken);
        return new EmptyResponse();
    }

    public override async Task<LikeResponse> Like(LikeRequest request, ServerCallContext context)
    {
        var projectId = Guid.Parse(request.ProjectId);
        var result = await _projectLikeService.LikeAsync(projectId, context.CancellationToken);

        return _mapper.Map<ProjectLikeDto, LikeResponse>(result);
    }
}