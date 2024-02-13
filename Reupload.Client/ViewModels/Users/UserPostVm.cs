using Reupload.Client.ViewModels.Posts;

namespace Reupload.Client.ViewModels.Users;

public class UserPostVm
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public IEnumerable<PostHashtagVm>? Hashtags { get; set; }

    public string ImageUri { get; set; } = string.Empty;
}