using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.Users;

public class UserPackageDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public double Price { get; set; }

    public int PhotoUploadLimit { get; set; }
    
    public static UserPackageDto FromPackage(Package package)
    {
        try
        {
            return new UserPackageDto
            {
                Id = package.Id,
                Name = package.Name,
                Price = package.Price,
                PhotoUploadLimit = package.PhotoUploadLimit
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(Package)} to {nameof(UserPackageDto)} failed.");
        }
    }
}