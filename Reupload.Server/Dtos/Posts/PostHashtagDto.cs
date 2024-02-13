using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.Posts;

public class PostHashtagDto
{
    public Guid? Id { get; set; }

    public string Text { get; set; } = default!;

    public static PostHashtagDto FromHashtag(Hashtag hashtag)
    {
        try
        {
            return new PostHashtagDto
            {
                Id = hashtag.Id,
                Text = hashtag.Text
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(Hashtag)} to {nameof(PostHashtagDto)} failed.");
        }
    }
}