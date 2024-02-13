using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;

namespace Reupload.Server.Dtos.Posts;

public class PostDetailsDto
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public IEnumerable<PostHashtagDto> Hashtags { get; set; } = default!;

    public PostUserDto User { get; set; } = default!;

    public string ImageUri { get; set; } = string.Empty;

    public static PostDetailsDto FromPost(Post post)
    {
        try
        {
            return new PostDetailsDto
            {
                Id = post.Id,
                Description = post.Description,
                CreatedAt = post.CreatedAt,
                Hashtags = post.Hashtags!.Select(PostHashtagDto.FromHashtag),
                User = PostUserDto.FromApplicationUser(post.ApplicationUser),
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(Post)} to {nameof(PostDetailsDto)} failed.");
        }
    }
}