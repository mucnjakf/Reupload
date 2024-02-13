using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reupload.Server.Dtos.Packages;
using Reupload.Server.Services.Contracts;

namespace Reupload.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/packages")]
public class PackageController : ControllerBase
{
    private readonly IPackageService _packageService;

    public PackageController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllDetailsAsync()
    {
        IEnumerable<PackageDetailsDto> packages = await _packageService.GetAllDetailsAsync();

        return Ok(packages);
    }
}