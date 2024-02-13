namespace Reupload.Server.Dtos.Posts;

public class PostUpdateDto
{
    public string? Description { get; set; }

    public List<PostHashtagDto>? Hashtags { get; set; }
}