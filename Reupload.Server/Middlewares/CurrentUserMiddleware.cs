using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Reupload.Server.Models;
using Reupload.Server.Services.Contracts;

namespace Reupload.Server.Middlewares;

public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Invoke(
        HttpContext httpContext,
        UserManager<ApplicationUser> userManager,
        ICurrentUserService currentUserService)
    {
        string? currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        currentUserService.ApplicationUser = await userManager.FindByIdAsync(currentUserId);

        await _next(httpContext);
    }
}