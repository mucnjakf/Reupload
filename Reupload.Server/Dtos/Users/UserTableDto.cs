using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.Users;

public class UserTableDto
{
    public string Id { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Fullname { get; set; } = default!;

    public string Package { get; set; } = default!;

    public int? PostsCount { get; set; }

    public static UserTableDto FromUser(ApplicationUser user)
    {
        try
        {
            return new UserTableDto
            {
                Id = user.Id,
                Username = user.UserName,
                Fullname = $"{user.FirstName} {user.LastName}",
                Package = user.Package.Name,
                PostsCount = user.Posts?.Count(),
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(ApplicationUser)} to {nameof(UserTableDto)} failed.");
        }
    }
}