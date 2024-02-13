using System.Net;
using System.Text.Json;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Dtos.Posts;
using Reupload.Server.Dtos.UserActions;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;
using Reupload.Server.Services.Contracts;
using Reupload.Shared.Constants;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserActionService _userActionService;
    private readonly IPackageService _packageService;

    public PostService(
        IUnitOfWork unitOfWork,
        IImageService imageService,
        ICurrentUserService currentUserService,
        IUserActionService userActionService,
        IPackageService packageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _currentUserService = currentUserService;
        _userActionService = userActionService;
        _packageService = packageService;
    }

    public async Task<PagedList<PostDetailsDto>> GetPaginatedDetailsAsync(PaginationRequestDto paginationRequestDto, string? userId = null)
    {
        PagedList<PostDetailsDto> paginatedPosts = await _unitOfWork.Posts.PaginatedDetailsAsync(paginationRequestDto, userId);

        foreach (PostDetailsDto paginatedPost in paginatedPosts)
        {
            try
            {
                paginatedPost.ImageUri = await _imageService.GetPostImageUriAsync(paginatedPost.User.Id, paginatedPost.Id);
            }
            catch (Exception)
            {
                Console.WriteLine($"Image for post ID: {paginatedPost.Id} not found.");
            }
        }

        return paginatedPosts;
    }

    public async Task<PostDetailsDto> GetDetailsAsync(Guid postId)
    {
        Post post = await GetPostAsync(postId);

        return PostDetailsDto.FromPost(post);
    }

    public async Task InsertAsync(PostInsertDto postInsertDto)
    {
        List<Hashtag> newHashtags = await GetHashtagsForPostAsync(postInsertDto.Hashtags);

        Post post = new()
        {
            Description = postInsertDto.Description,
            Hashtags = newHashtags,
            ApplicationUserId = _currentUserService.ApplicationUser.Id
        };

        await _packageService.IncrementConsumptionAsync(_currentUserService.ApplicationUser.Id);

        await _unitOfWork.Posts.AddAsync(post);
        await _unitOfWork.SaveChangesAsync();

        await _imageService.InsertPostImageAsync(post.ApplicationUserId, post.Id, postInsertDto.Base64Image, postInsertDto.ContentType);

        await _userActionService.InsertAsync(new UserActionInsertDto
        {
            UserId = _currentUserService.ApplicationUser.Id,
            Timestamp = DateTime.Now,
            ActionType = ActionType.CreateNewPost,
            ActionParametersJson = JsonSerializer.Serialize(new CreateNewPostUserActionDto
            {
                Description = postInsertDto.Description,
                Hashtags = newHashtags
            })
        });
    }

    public async Task UpdateAsync(Guid postId, PostUpdateDto postUpdateDto)
    {
        List<Hashtag> newHashtags = await GetHashtagsForPostAsync(postUpdateDto.Hashtags);

        Post post = await GetPostAsync(postId);
        post.Description = postUpdateDto.Description;
        post.Hashtags = newHashtags;

        await _unitOfWork.SaveChangesAsync();

        await _userActionService.InsertAsync(new UserActionInsertDto
        {
            UserId = _currentUserService.ApplicationUser.Id,
            Timestamp = DateTime.Now,
            ActionType = ActionType.UpdatePost,
            ActionParametersJson = JsonSerializer.Serialize(new UpdatePostUserActionDto
            {
                PostId = postId,
                Description = postUpdateDto.Description,
                Hashtags = newHashtags.Select(x => new PostHashtagDto { Id = x.Id, Text = x.Text })
            })
        });
    }

    private async Task<List<Hashtag>> GetHashtagsForPostAsync(List<PostHashtagDto>? hashtagDtos)
    {
        List<Hashtag> allHashtags = await _unitOfWork.Hashtags.FindAsync();
        List<PostHashtagDto> newHashtagDtos = new();
        List<Hashtag> existingHashtags = new();

        if (hashtagDtos is not null)
        {
            newHashtagDtos = hashtagDtos
                .Where(x => allHashtags.All(y => y.Text != x.Text))
                .ToList();

            existingHashtags = allHashtags
                .Where(x => hashtagDtos.Any(y => y.Text == x.Text))
                .ToList();
        }

        List<Hashtag> newHashtags = new(newHashtagDtos.Select(x => new Hashtag { Text = x.Text }));
        newHashtags.AddRange(existingHashtags);

        return newHashtags;
    }

    public async Task DeleteAsync(Guid postId)
    {
        await _packageService.DecrementConsumptionAsync(_currentUserService.ApplicationUser.Id);

        await _imageService.DeletePostImageAsync(_currentUserService.ApplicationUser.Id, postId);

        Post post = await GetPostAsync(postId);

        _unitOfWork.Posts.Remove(post);
        await _unitOfWork.SaveChangesAsync();

        await _userActionService.InsertAsync(new UserActionInsertDto
        {
            UserId = _currentUserService.ApplicationUser.Id,
            Timestamp = DateTime.Now,
            ActionType = ActionType.DeletePost,
            ActionParametersJson = JsonSerializer.Serialize(new DeletePostUserActionDto
            {
                PostId = postId,
            })
        });
    }

    private async Task<Post> GetPostAsync(Guid postId)
    {
        Post? post = await _unitOfWork.Posts.FirstDetailsAsync(x => x.Id == postId);

        if (post is null)
        {
            throw new PostException(ErrorCode.PostNotFound, HttpStatusCode.NotFound, $"Post with ID {postId} not found.");
        }

        return post;
    }
}