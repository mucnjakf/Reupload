namespace Reupload.Client.ViewModels.Posts;

public class PostDetailsVm
{
    public Guid Id { get; set; }

    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public IEnumerable<PostHashtagVm>? Hashtags { get; set; } = default!;

    public PostUserVm User { get; set; } = default!;

    public string ImageUri { get; set; } = default!;
}