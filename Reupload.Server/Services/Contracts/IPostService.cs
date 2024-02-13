using Reupload.Server.Dtos;
using Reupload.Server.Dtos.Posts;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Services.Contracts;

public interface IPostService
{
    Task<PagedList<PostDetailsDto>> GetPaginatedDetailsAsync(PaginationRequestDto paginationRequestDto, string? userId = null);

    Task<PostDetailsDto> GetDetailsAsync(Guid postId);

    Task InsertAsync(PostInsertDto postInsertDto);

    Task UpdateAsync(Guid postId, PostUpdateDto postUpdateDto);

    Task DeleteAsync(Guid postId);
}