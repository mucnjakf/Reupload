using System.Linq.Expressions;
using Reupload.Server.Dtos.UserActions;
using Reupload.Server.Models;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Data.Repositories.Contracts;

public interface IUserActionRepository : IRepository<UserAction>
{
    Task<PagedList<UserActionTableDto>> PaginatedDetailsAsync(PaginationRequestDto paginationRequestDto);

    Task<UserAction?> FirstDetailsAsync(Expression<Func<UserAction, bool>> expression);
}