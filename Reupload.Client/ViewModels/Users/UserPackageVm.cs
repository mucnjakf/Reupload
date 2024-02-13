namespace Reupload.Client.ViewModels.Users;

public class UserPackageVm
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public double Price { get; set; }

    public int PhotoUploadLimit { get; set; }
}