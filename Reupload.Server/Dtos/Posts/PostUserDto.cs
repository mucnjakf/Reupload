using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.Posts;

public class PostUserDto
{
    public string Id { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public PostPackageDto Package { get; set; } = default!;
    
    public static PostUserDto FromApplicationUser(ApplicationUser applicationUser)
    {
        try
        {
            return new PostUserDto
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Package = PostPackageDto.FromPackage(applicationUser.Package)
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(ApplicationUser)} to {nameof(PostUserDto)} failed.");
        }
    }
}