using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Projectiv.IdentityService.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestV1Controller : ControllerBase
{
    [HttpGet]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {   
        //var tt = HttpContext.User.Identity.IsAuthenticated;
        //Console.WriteLine(tt);

        return Ok(new { Test = "test" });
    }
}