namespace Reupload.Server.Dtos.Users;

public class UserChangePackageDto
{
    public string UserId { get; set; } = default!;

    public Guid PackageId { get; set; }
}