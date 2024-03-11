using AutoMapper;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Project;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.PetprojectsService.Domain.Managers;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.EntityFramework.Models.FilterModels;
using Projectvil.Shared.EntityFramework.Translations;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.Application.Services.PetProjectService;

public class ProjectService : IProjectService, ITransientDependence
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly IProjectDocumentService _productDocumentService;
    private readonly IStatusAppService _statusAppService;
    private readonly IDomainAppService _domainAppService;
    private readonly IProjectManager _projectManager;
    private readonly IProjectLikeService _productLikeService;

    public ProjectService(IProjectRepository projectRepository,
        IMapper mapper, 
        IProjectDocumentService projectDocumentService,
        IStatusAppService statusAppService, 
        IDomainAppService domainAppService, 
        IProjectManager projectManager,
        IProjectLikeService productLikeService)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _productDocumentService = projectDocumentService;
        _statusAppService = statusAppService;
        _domainAppService = domainAppService;
        _projectManager = projectManager;
        _productLikeService = productLikeService;
    }

    public async Task<ProjectDto> GetAsync(Guid id, ELanguage language, CancellationToken cancellationToken = default)
    {
        var model = await _projectRepository.GetAsync(x => x.Id == id, language, cancellationToken);
        var documents = await _productDocumentService.GetDocumentsManyAsync(model.Id, cancellationToken);
        
        var view = _mapper.Map<Project, ProjectDto>(model);
        view.DocumentsIds = documents;

        return view;
    }

    public async Task<PageResultDto<ProjectDto>> GetListAsync(ProjectListInput input, ELanguage language, CancellationToken cancellationToken = default)
    {
        var models = await _projectRepository.GetListAsync(input, language, cancellationToken);
        var totalCount = await _projectRepository.GetCountAsync(input, language, cancellationToken);
        var views = _mapper.Map<List<Project>, List<ProjectDto>>(models);

        var likes = await _productLikeService.GetListAsync(cancellationToken);
        views.ForEach(x => x.Like = likes.FirstOrDefault(y => y.ProjectId == x.Id));
        
        return new PageResultDto<ProjectDto>(totalCount, views);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectInput input, ELanguage language, CancellationToken cancellationToken = default)
    {
        var model = _mapper.Map<CreateProjectInput, Project>(input);
        await _projectRepository.InsertAsync(model, cancellationToken: cancellationToken);
        await _productDocumentService.CreateManyAsync(input.DocumentsIds, model.Id, cancellationToken);

        var status = await _statusAppService.GetAsync(input.StatusId, language, cancellationToken);
        var domain = await _domainAppService.GetAsync(input.DomainId, language, cancellationToken);
        
        var view = _mapper.Map<Project, ProjectDto>(model);
        view.DocumentsIds = input.DocumentsIds;
        view.StatusName = status.Name;
        view.DomainName = domain.Name;

        return view;
    }

    public async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectInput input, ELanguage language,
        CancellationToken cancellationToken = default)
    {
        var model = await _projectRepository.GetAsync(x => x.Id == id, cancellationToken);
        model = _mapper.Map(input, model);

        model = await _projectRepository.UpdateAsync(model, cancellationToken: cancellationToken);
        var documentsIds =
            await _productDocumentService.CreateOrDeleteAsync(input.DocumentsIds, model.Id, cancellationToken);
        
        var status = await _statusAppService.GetAsync(model.StatusId, language, cancellationToken);
        var domain = await _domainAppService.GetAsync(model.DomainId, language, cancellationToken);
        
        var view = _mapper.Map<Project, ProjectDto>(model);
        view.DocumentsIds = documentsIds;
        view.StatusName = status.Name;
        view.DomainName = domain.Name;

        return view;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _projectManager.DeleteAsync(id, cancellationToken);
    }
}