namespace Reupload.Server.Models;

public class PackageConsumption
{
    public Guid Id { get; set; }

    public Guid PackageId { get; set; }

    public Package Package { get; set; } = default!;

    public string UserId { get; set; } = default!;

    public ApplicationUser User { get; set; } = default!;

    public int PhotoUploadAmount { get; set; } = 0;
}