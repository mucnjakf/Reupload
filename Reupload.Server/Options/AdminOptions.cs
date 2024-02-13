namespace Reupload.Server.Options;

public class AdminOptions
{
    public const string SectionName = "AdminOptions";

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;
}