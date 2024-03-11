using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projectiv.IdentityService.ApplicationShared.Interfaces.Services;

namespace Projectiv.IdentityService.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserBlobController : ControllerBase
{
    private readonly IUserBlobService _userBlobService;

    public UserBlobController(IUserBlobService userBlobService)
    {
        _userBlobService = userBlobService;
    }

    [HttpGet]
    [AllowAnonymous] // change to Authorize
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _userBlobService.GetAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous] // change to Authorize
    public async Task<IActionResult> CreateAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var result = await _userBlobService.CreateAsync(file, cancellationToken);

        return Ok(result);
    }
}