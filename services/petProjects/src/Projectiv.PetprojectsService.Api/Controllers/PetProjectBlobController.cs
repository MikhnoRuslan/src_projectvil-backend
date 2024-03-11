using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectvil.Shared.EntityFramework.Blob.Attributes;

namespace Projectiv.PetprojectsService.Api.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("[controller]/[action]")]
public class PetProjectBlobController : ControllerBase
{
    private readonly IPetProjectBlobService _petProjectBlobService;

    public PetProjectBlobController(IPetProjectBlobService petProjectBlobService)
    {
        _petProjectBlobService = petProjectBlobService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _petProjectBlobService.GetAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [RequestSizeLimit(5 * 1024 * 1024)]
    [AllowedExtensions("jpg,jpeg,png,svg")]
    public async Task<IActionResult> CreateAsync(IFormFile file,
        CancellationToken cancellationToken)
    {
        var result = await _petProjectBlobService.CreateAsync(file, cancellationToken);

        return Ok(result);
    }

    [HttpPatch]
    [RequestSizeLimit(5 * 1024 * 1024)]
    [AllowedExtensions("jpg,jpeg,png,svg")]
    public async Task<IActionResult> UpdateAsync(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        var result = await _petProjectBlobService.UpdateAsync(id, file, cancellationToken);

        return Ok(result);
    }

    [HttpDelete]
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _petProjectBlobService.DeleteAsync(id, cancellationToken);
    }
}