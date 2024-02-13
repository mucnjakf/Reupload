using Reupload.Server.Dtos.Users;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Services.Contracts;

public interface IUserService
{
    Task<UserDetailsDto> GetCurrentUserDetailsAsync();

    Task UpdateUserPackageAsync(string userId, Guid packageId);

    Task<PagedList<UserTableDto>> GetPaginatedTableAsync(PaginationRequestDto paginationRequestDto);

    Task<UserDetailsDto> GetDetailsAsync(string userId);

    Task UpdateAsync(string userId, UserUpdateDto userUpdateDto);

    Task DeleteAsync(string userId);
}