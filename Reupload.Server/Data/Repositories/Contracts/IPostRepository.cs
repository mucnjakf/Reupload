using System.Linq.Expressions;
using Reupload.Server.Dtos;
using Reupload.Server.Dtos.Posts;
using Reupload.Server.Models;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Data.Repositories.Contracts;

public interface IPostRepository : IRepository<Post>
{
    Task<PagedList<PostDetailsDto>> PaginatedDetailsAsync(PaginationRequestDto paginationRequestDto, string? userId = null);

    Task<Post?> FirstDetailsAsync(Expression<Func<Post, bool>> expression);
}