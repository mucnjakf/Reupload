namespace Reupload.Server.Models;

public class Post
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public IEnumerable<Hashtag>? Hashtags { get; set; }

    public string ApplicationUserId { get; set; } = default!;

    public ApplicationUser ApplicationUser { get; set; } = default!;
}