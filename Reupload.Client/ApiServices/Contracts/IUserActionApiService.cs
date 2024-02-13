using Reupload.Client.ViewModels;
using Reupload.Client.ViewModels.UserActions;
using Reupload.Shared.Pagination;

namespace Reupload.Client.ApiServices.Contracts;

public interface IUserActionApiService
{
    Task<ResultResponseVm<PaginationResponseDto<UserActionTableVm>>> GetPaginatedTableAsync(
        PaginationRequestDto paginationRequestDto);

    Task<ResultResponseVm<UserActionDetailsVm>> GetDetailsAsync(Guid userActionId);
}