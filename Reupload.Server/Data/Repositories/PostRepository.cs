using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Dtos.Posts;
using Reupload.Server.Models;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Data.Repositories;

public class PostRepository : Repository<ApplicationDbContext, Post>, IPostRepository
{
    public PostRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PagedList<PostDetailsDto>> PaginatedDetailsAsync(PaginationRequestDto paginationRequestDto, string? userId = null)
    {
        IQueryable<Post> query = DbContext.Set<Post>();

        query = query
            .Include(x => x.Hashtags)
            .Include(x => x.ApplicationUser)
            .ThenInclude(x => x.Package);

        if (userId is not null)
        {
            query = query.Where(x => x.ApplicationUserId == userId);
        }

        query = Search(query, paginationRequestDto.SearchQuery);

        int totalCount = await query.CountAsync();

        List<PostDetailsDto> posts = query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((paginationRequestDto.PageNumber - 1) * paginationRequestDto.PageSize)
            .Take(paginationRequestDto.PageSize)
            .AsEnumerable()
            .Select(PostDetailsDto.FromPost)
            .ToList();

        return new PagedList<PostDetailsDto>(posts, totalCount, paginationRequestDto.PageNumber, paginationRequestDto.PageSize);
    }

    public async Task<Post?> FirstDetailsAsync(Expression<Func<Post, bool>> expression)
    {
        IQueryable<Post> query = DbContext.Set<Post>();

        return await query
            .Include(x => x.Hashtags)
            .Include(x => x.ApplicationUser)
            .ThenInclude(x => x.Package)
            .FirstOrDefaultAsync(expression);
    }

    private static IQueryable<Post> Search(IQueryable<Post> query, string? searchQuery)
    {
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToUpper();

            query = query.Where(x => x.ApplicationUser.FirstName.ToUpper().Contains(searchQuery)
                                     || x.ApplicationUser.LastName.ToUpper().Contains(searchQuery)
                                     || x.Hashtags!.Any(y => y.Text == searchQuery));
        }

        return query;
    }
}