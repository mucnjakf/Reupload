using System.Text.Json.Serialization;

namespace Reupload.Server.Dtos.Posts;

public class PostInsertDto
{
    public string? Description { get; set; }

    public List<PostHashtagDto>? Hashtags { get; set; }

    public string Base64Image { get; set; } = default!;

    public string ContentType { get; set; } = default!;

    public string? FileName { get; set; } = default!;

    [JsonIgnore] public string? UserId { get; set; }

    [JsonIgnore] public Guid PostId { get; set; }
}