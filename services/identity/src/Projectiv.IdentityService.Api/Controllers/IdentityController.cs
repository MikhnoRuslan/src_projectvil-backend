using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projectiv.IdentityService.ApplicationShared.Inputs.User;
using Projectiv.IdentityService.ApplicationShared.Interfaces.Services;

namespace Projectiv.IdentityService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityAppService _identityAppService;

    public IdentityController(IIdentityAppService identityAppService)
    {
        _identityAppService = identityAppService;
    }

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAsync(CreateUserInput input, CancellationToken cancellationToken)
    {
        await _identityAppService.CreateAsync(input, cancellationToken);

        return Ok(1);
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordInput input, CancellationToken cancellationToken)
    {
        await _identityAppService.ResetPasswordAsync(input, cancellationToken);

        return Ok();
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailInput input, CancellationToken cancellationToken)
    {
        var result = await _identityAppService.ConfirmEmailAsync(input, cancellationToken);

        return Redirect(result + "/login");
    }
}