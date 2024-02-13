namespace Reupload.Client.ViewModels.Users;

public class UserTableVm
{
    public string Id { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Fullname { get; set; } = default!;

    public string Package { get; set; } = default!;

    public int? PostsCount { get; set; }
}