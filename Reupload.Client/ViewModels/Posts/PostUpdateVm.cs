namespace Reupload.Client.ViewModels.Posts;

public class PostUpdateVm
{
    public string? Description { get; set; }

    public List<PostHashtagVm>? Hashtags { get; set; }
}