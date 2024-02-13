using System.Net;
using Reupload.Server.Dtos.Posts;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.Users;

public class UserPostDto
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public IEnumerable<PostHashtagDto>? Hashtags { get; set; }

    public string ImageUri { get; set; } = string.Empty;

    public static UserPostDto FromPost(Post post)
    {
        try
        {
            return new UserPostDto
            {
                Id = post.Id,
                Description = post.Description,
                CreatedAt = post.CreatedAt,
                Hashtags = post.Hashtags?.Select(PostHashtagDto.FromHashtag)
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(Post)} to {nameof(UserPostDto)} failed.");
        }
    }
}