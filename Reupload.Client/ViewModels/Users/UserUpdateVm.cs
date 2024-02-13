namespace Reupload.Client.ViewModels.Users;

public class UserUpdateVm
{
    public string Username { get; set; } = default!;
    
    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
}