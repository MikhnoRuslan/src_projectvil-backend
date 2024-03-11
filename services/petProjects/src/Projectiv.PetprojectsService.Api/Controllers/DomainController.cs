using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Domains;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectvil.Shared.EntityFramework.Models.FilterModels;

namespace Projectiv.PetprojectsService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DomainController : ControllerBase
{
    private readonly IDomainAppService _domainAppService;

    public DomainController(IDomainAppService domainAppService)
    {
        _domainAppService = domainAppService;
    }
    
    [HttpGet("domains")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [AllowAnonymous]
    public async Task<PageResultDto<DomainDto>> GetListAsync([FromQuery] DomainListInput input, CancellationToken cancellationToken)
    {
        return await _domainAppService.GetListAsync(input, cancellationToken);
    }
}