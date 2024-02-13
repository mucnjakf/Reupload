using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.UserActions;

public class UserActionUser
{
    public string Id { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public static UserActionUser FromUser(ApplicationUser user)
    {
        try
        {
            return new UserActionUser
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(ApplicationUser)} to {nameof(UserActionUser)} failed.");
        }
    }
}