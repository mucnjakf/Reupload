using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reupload.Server.Dtos.Users;
using Reupload.Server.Services.Contracts;
using Reupload.Shared.Constants;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class ProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("current-user")]
    public async Task<IActionResult> GetCurrentUserDetailsAsync()
    {
        UserDetailsDto userDetailsDto = await _userService.GetCurrentUserDetailsAsync();

        return Ok(userDetailsDto);
    }

    [HttpPut("change-package")]
    public async Task<IActionResult> UpdateUserPackageAsync([FromBody] UserChangePackageDto userChangePackageDto)
    {
        await _userService.UpdateUserPackageAsync(userChangePackageDto.UserId, userChangePackageDto.PackageId);

        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("table")]
    public async Task<IActionResult> GetPaginatedTableAsync([FromQuery] PaginationRequestDto paginationRequestDto)
    {
        PagedList<UserTableDto> users = await _userService.GetPaginatedTableAsync(paginationRequestDto);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(users.Metadata));

        return Ok(users);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetDetailsAsync([FromRoute] string userId)
    {
        UserDetailsDto user = await _userService.GetDetailsAsync(userId);

        return Ok(user);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string userId, UserUpdateDto userUpdateDto)
    {
        await _userService.UpdateAsync(userId, userUpdateDto);

        return NoContent();
    }
    
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string userId)
    {
        await _userService.DeleteAsync(userId);

        return NoContent();
    }
}