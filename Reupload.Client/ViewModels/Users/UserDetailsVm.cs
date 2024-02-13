namespace Reupload.Client.ViewModels.Users;

public class UserDetailsVm
{
    public string Id { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public UserPackageVm Package { get; set; } = default!;

    public bool PackageIsActive { get; set; }
    
    public int PhotoUploadAmount { get; set; }
}