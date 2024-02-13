using System.ComponentModel.DataAnnotations;

namespace Reupload.Client.ViewModels.Posts;

public class PostInsertVm
{
    public string? Description { get; set; }

    public List<PostHashtagVm>? Hashtags { get; set; }
    
    public string? Base64Image { get; set; }

    public string? ContentType { get; set; }

    public string? FileName { get; set; }
}