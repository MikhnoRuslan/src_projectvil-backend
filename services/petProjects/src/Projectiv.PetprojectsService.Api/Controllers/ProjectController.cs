using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Project;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Models.FilterModels;

namespace Projectiv.PetprojectsService.Api.Controllers;

// [Authorize(AuthenticationSchemes = "Bearer")]
[Route("[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILanguageHandler _languageHandler;

    public ProjectController(IProjectService projectService, 
        ILanguageHandler languageHandler)
    {
        _projectService = projectService;
        _languageHandler = languageHandler;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var language = _languageHandler.GetLanguage();
        var result = await _projectService.GetAsync(id, language, cancellationToken);

        return Ok(result);
    }

    [HttpGet("projects")]
    public async Task<PageResultDto<ProjectDto>> GetListAsync([FromQuery] ProjectListInput input,
        CancellationToken cancellationToken)
    {
        var language = _languageHandler.GetLanguage();
        return await _projectService.GetListAsync(input, language, cancellationToken);
    }

    [HttpPost("create-project")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProjectInput input, CancellationToken cancellationToken)
    {
        var language = _languageHandler.GetLanguage();
        var result = await _projectService.CreateAsync(input, language, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("like")]
    public async Task<IActionResult> LikeAsync([FromBody] Guid projectId,
        [FromServices] IProjectLikeService likeService,
        CancellationToken cancellationToken)
    {
        var result = await likeService.LikeAsync(projectId, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("update-project/{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateProjectInput input, CancellationToken cancellationToken)
    {
        var language = _languageHandler.GetLanguage();
        var result = await _projectService.UpdateAsync(id, input, language, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _projectService.DeleteAsync(id, cancellationToken);
    }
}