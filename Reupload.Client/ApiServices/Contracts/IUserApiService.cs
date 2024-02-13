using Reupload.Client.ViewModels;
using Reupload.Client.ViewModels.Users;
using Reupload.Shared.Pagination;

namespace Reupload.Client.ApiServices.Contracts;

public interface IUserApiService
{
    Task<ResultResponseVm<UserDetailsVm>> GetCurrentUserDetailsAsync();

    Task<EmptyResponseVm> UpdateUserPackageAsync(UserChangePackageVm userChangePackageVm);

    Task<ResultResponseVm<PaginationResponseDto<UserTableVm>>> GetPaginatedTableAsync(
        PaginationRequestDto paginationRequestDto);

    Task<ResultResponseVm<UserDetailsVm>> GetDetailsAsync(string userId);

    Task<EmptyResponseVm> UpdateAsync(string userId, UserUpdateVm userUpdateVm);

    Task<EmptyResponseVm> DeleteAsync(string userId);
}