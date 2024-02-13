using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reupload.Server.Dtos.UserActions;
using Reupload.Server.Services.Contracts;
using Reupload.Shared.Constants;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Controllers;

[ApiController]
[Authorize(Roles = Roles.Admin)]
[Route("api/user-actions")]
public class UserActionController : ControllerBase
{
    private readonly IUserActionService _userActionService;

    public UserActionController(IUserActionService userActionService)
    {
        _userActionService = userActionService;
    }
    
    [HttpGet("table")]
    public async Task<IActionResult> GetPaginatedTableAsync([FromQuery] PaginationRequestDto paginationRequestDto)
    {
        PagedList<UserActionTableDto> users = await _userActionService.GetPaginatedTableAsync(paginationRequestDto);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(users.Metadata));

        return Ok(users);
    }

    [HttpGet("{userActionId:guid}")]
    public async Task<IActionResult> GetDetailsAsync([FromRoute] Guid userActionId)
    {
        UserActionDetailsDto userAction = await _userActionService.GetDetailsAsync(userActionId);

        return Ok(userAction);
    }
}