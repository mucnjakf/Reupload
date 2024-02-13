namespace Reupload.Server.Dtos.UserActions;

public class RegisterUserActionDto
{
    public string Username { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public Guid PackageId { get; set; }
}