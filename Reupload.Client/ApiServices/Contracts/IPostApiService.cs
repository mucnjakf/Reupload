using Reupload.Client.ViewModels;
using Reupload.Client.ViewModels.Posts;
using Reupload.Client.ViewModels.Users;
using Reupload.Shared.Pagination;

namespace Reupload.Client.ApiServices.Contracts;

public interface IPostApiService
{
    Task<ResultResponseVm<PaginationResponseDto<PostDetailsVm>>> GetPaginatedDetailsAsync(
        PaginationRequestDto paginationRequestDto);

    Task<Tuple<Stream, string>?> GetPostImageAsync(string userId, Guid postId);

    Task<ResultResponseVm<PaginationResponseDto<UserPostVm>>> GetPaginatedForUserDetailsAsync(
        PaginationRequestDto paginationRequestDto, string? userId);

    Task<EmptyResponseVm> InsertAsync(PostInsertVm postInsertVm);

    Task<EmptyResponseVm> UpdateAsync(Guid postId, PostUpdateVm postUpdateVm);

    Task<EmptyResponseVm> DeleteAsync(Guid postId);
}