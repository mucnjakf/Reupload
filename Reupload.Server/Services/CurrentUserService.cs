using Reupload.Server.Models;
using Reupload.Server.Services.Contracts;

namespace Reupload.Server.Services;

public class CurrentUserService : ICurrentUserService
{
    public ApplicationUser ApplicationUser { get; set; } = default!;
}