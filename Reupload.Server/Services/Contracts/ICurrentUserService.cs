using Reupload.Server.Models;

namespace Reupload.Server.Services.Contracts;

public interface ICurrentUserService
{
    ApplicationUser ApplicationUser { get; set; }
}