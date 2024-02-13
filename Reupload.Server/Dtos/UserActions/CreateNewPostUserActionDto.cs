using Reupload.Server.Models;

namespace Reupload.Server.Dtos.UserActions;

public class CreateNewPostUserActionDto
{
    public string? Description { get; set; }

    public IEnumerable<Hashtag>? Hashtags { get; set; }
}