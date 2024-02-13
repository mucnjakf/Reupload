using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Dtos.UserActions;
using Reupload.Server.Models;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Data.Repositories;

public class UserActionRepository : Repository<ApplicationDbContext, UserAction>, IUserActionRepository
{
    public UserActionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PagedList<UserActionTableDto>> PaginatedDetailsAsync(PaginationRequestDto paginationRequestDto)
    {
        IQueryable<UserAction> query = DbContext.Set<UserAction>();

        query = query
            .Include(x => x.ApplicationUser);

        int totalCount = await query.CountAsync();

        List<UserActionTableDto> userActionTableDtos = query
            .Skip((paginationRequestDto.PageNumber - 1) * paginationRequestDto.PageSize)
            .Take(paginationRequestDto.PageSize)
            .AsEnumerable()
            .Select(UserActionTableDto.FromUserAction)
            .ToList();

        return new PagedList<UserActionTableDto>(userActionTableDtos, totalCount, paginationRequestDto.PageNumber,
            paginationRequestDto.PageSize);
    }

    public async Task<UserAction?> FirstDetailsAsync(Expression<Func<UserAction, bool>> expression)
    {
        IQueryable<UserAction> query = DbContext.Set<UserAction>();

        return await query
            .Include(x => x.ApplicationUser)
            .FirstOrDefaultAsync(expression);
    }
}