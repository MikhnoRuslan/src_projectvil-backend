using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Statuses;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectvil.Shared.EntityFramework.Models.FilterModels;

namespace Projectiv.PetprojectsService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly IStatusAppService _statusAppService;

    public StatusController(IStatusAppService statusAppService)
    {
        _statusAppService = statusAppService;
    }

    [HttpGet("statuses")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [AllowAnonymous]
    public async Task<PageResultDto<StatusDto>> GetListAsync([FromQuery] StatusListInput input, CancellationToken cancellationToken)
    {
        return await _statusAppService.GetListAsync(input, cancellationToken);
    }
}