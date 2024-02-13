namespace Reupload.Server.Models;

public class Hashtag
{
    public Guid Id { get; set; }

    public string Text { get; set; } = default!;

    public IEnumerable<Post>? Posts { get; set; }
}