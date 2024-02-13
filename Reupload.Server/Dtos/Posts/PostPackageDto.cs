using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.Posts;

public class PostPackageDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public double Price { get; set; }

    public static PostPackageDto FromPackage(Package package)
    {
        try
        {
            return new PostPackageDto
            {
                Id = package.Id,
                Name = package.Name,
                Price = package.Price
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(Package)} to {nameof(PostPackageDto)} failed.");
        }
    }
}