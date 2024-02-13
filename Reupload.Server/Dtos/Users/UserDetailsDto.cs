using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.Users;

public class UserDetailsDto
{
    public string Id { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public UserPackageDto Package { get; set; } = default!;

    public bool PackageIsActive { get; set; }

    public int PhotoUploadAmount { get; set; }

    public static UserDetailsDto FromApplicationUser(ApplicationUser applicationUser)
    {
        try
        {
            return new UserDetailsDto
            {
                Id = applicationUser.Id,
                Username = applicationUser.UserName,
                Email = applicationUser.Email,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Package = UserPackageDto.FromPackage(applicationUser.Package),
                PackageIsActive = applicationUser.PackageIsActive,
                PhotoUploadAmount = applicationUser.PackageConsumption.PhotoUploadAmount
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(ApplicationUser)} to {nameof(UserDetailsDto)} failed.");
        }
    }
}