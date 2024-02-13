namespace Reupload.Server.Dtos.Users;

public class UserUpdateDto
{
    public string Username { get; set; } = default!;
    
    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
}