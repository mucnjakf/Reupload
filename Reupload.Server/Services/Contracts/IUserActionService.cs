using Reupload.Server.Dtos.UserActions;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Services.Contracts;

public interface IUserActionService
{
    Task InsertAsync(UserActionInsertDto userActionInsertDto);

    Task<PagedList<UserActionTableDto>> GetPaginatedTableAsync(PaginationRequestDto paginationRequestDto);

    Task<UserActionDetailsDto> GetDetailsAsync(Guid userActionId);
}