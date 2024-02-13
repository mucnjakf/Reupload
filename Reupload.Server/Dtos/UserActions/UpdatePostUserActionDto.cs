using Reupload.Server.Dtos.Posts;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.UserActions;

public class UpdatePostUserActionDto
{
    public Guid PostId { get; set; }

    public string? Description { get; set; }

    public IEnumerable<PostHashtagDto>? Hashtags { get; set; }
}