using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reupload.Server.Dtos.Posts;
using Reupload.Server.Services.Contracts;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/posts")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IImageService _imageService;

    public PostController(IPostService postService, IImageService imageService)
    {
        _postService = postService;
        _imageService = imageService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPaginatedDetailsAsync([FromQuery] PaginationRequestDto paginationRequestDto)
    {
        PagedList<PostDetailsDto> posts = await _postService.GetPaginatedDetailsAsync(paginationRequestDto);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(posts.Metadata));

        return Ok(posts);
    }

    [HttpGet("{userId}/{postId:guid}/image")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPostImageStreamAsync([FromRoute] string userId, [FromRoute] Guid postId)
    {
        Tuple<Stream, string> imageData = await _imageService.GetPostImageStreamAsync(userId, postId);

        return File(imageData.Item1, $"image/{imageData.Item2}");
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPaginatedForUserDetailsAsync(
        [FromRoute] string userId, [FromQuery] PaginationRequestDto paginationRequestDto)
    {
        PagedList<PostDetailsDto> posts = await _postService.GetPaginatedDetailsAsync(paginationRequestDto, userId);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(posts.Metadata));

        return Ok(posts);
    }

    [HttpGet("{postId:guid}")]
    public async Task<IActionResult> GetDetailsAsync([FromRoute] Guid postId)
    {
        PostDetailsDto postDetailsDto = await _postService.GetDetailsAsync(postId);

        return Ok(postDetailsDto);
    }

    [HttpPost]
    public async Task<IActionResult> InsertAsync([FromBody] PostInsertDto postInsertDto)
    {
        await _postService.InsertAsync(postInsertDto);

        return NoContent();
    }

    [HttpPut("{postId:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid postId, PostUpdateDto postUpdateDto)
    {
        await _postService.UpdateAsync(postId, postUpdateDto);

        return NoContent();
    }

    [HttpDelete("{postId:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid postId)
    {
        await _postService.DeleteAsync(postId);

        return NoContent();
    }
}