using System.Net;
using System.Text.Json;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Dtos.UserActions;
using Reupload.Server.Dtos.Users;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;
using Reupload.Server.Services.Contracts;
using Reupload.Shared.Constants;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserActionService _userActionService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        IUserActionService userActionService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _userActionService = userActionService;
    }

    public async Task<UserDetailsDto> GetCurrentUserDetailsAsync()
    {
        ApplicationUser user = await GetUserAsync(_currentUserService.ApplicationUser.Id);

        return UserDetailsDto.FromApplicationUser(user);
    }


    public async Task UpdateUserPackageAsync(string userId, Guid packageId)
    {
        ApplicationUser user = await GetUserAsync(userId);

        bool packageExists = await _unitOfWork.Packages.AnyAsync(x => x.Id == packageId);

        if (!packageExists)
        {
            throw new PackageException(ErrorCode.PackageNotFound, HttpStatusCode.NotFound, $"Package with ID {packageId} not found.");
        }

        user.PackageIsActive = false;
        await _userManager.UpdateAsync(user);

        BackgroundJob.Schedule(() => UpdatePackageAfter24Hours(userId, packageId), TimeSpan.FromSeconds(5));
    }

    public async Task UpdatePackageAfter24Hours(string userId, Guid packageId)
    {
        ApplicationUser user = await GetUserAsync(userId);
        Guid oldPackageId = user.PackageId;

        user.PackageId = packageId;
        user.PackageIsActive = true;

        int oldPhotoUploadAmount = user.PackageConsumption.PhotoUploadAmount;

        user.PackageConsumption = new PackageConsumption
        {
            PackageId = packageId,
            UserId = userId,
            PhotoUploadAmount = oldPhotoUploadAmount
        };

        await _userManager.UpdateAsync(user);

        await _userActionService.InsertAsync(new UserActionInsertDto
        {
            UserId = userId,
            Timestamp = DateTime.Now,
            ActionType = ActionType.ChangePackage,
            ActionParametersJson = JsonSerializer.Serialize(new UpdatePackageUserActionDto
            {
                OldPackageId = oldPackageId,
                NewPackageId = packageId
            })
        });
    }

    public async Task<PagedList<UserTableDto>> GetPaginatedTableAsync(PaginationRequestDto paginationRequestDto)
    {
        IQueryable<ApplicationUser> query = _userManager.Users
            .Include(x => x.Posts)
            .Include(x => x.Package);

        int totalCount = await query.CountAsync();

        List<UserTableDto> users = query
            .Skip((paginationRequestDto.PageNumber - 1) * paginationRequestDto.PageSize)
            .Take(paginationRequestDto.PageSize)
            .AsEnumerable()
            .Select(UserTableDto.FromUser)
            .ToList();

        return new PagedList<UserTableDto>(users, totalCount, paginationRequestDto.PageNumber, paginationRequestDto.PageSize);
    }

    public async Task<UserDetailsDto> GetDetailsAsync(string userId)
    {
        ApplicationUser user = await GetUserAsync(userId);

        return UserDetailsDto.FromApplicationUser(user);
    }

    public async Task UpdateAsync(string userId, UserUpdateDto userUpdateDto)
    {
        ApplicationUser user = await GetUserAsync(userId);

        user.UserName = userUpdateDto.Username;
        user.NormalizedUserName = userUpdateDto.Username.ToUpper();
        user.Email = userUpdateDto.Email;
        user.EmailConfirmed = false;
        user.NormalizedEmail = userUpdateDto.Email.ToUpper();
        user.FirstName = userUpdateDto.FirstName;
        user.LastName = userUpdateDto.LastName;

        await _userManager.UpdateAsync(user);
    }

    public async Task DeleteAsync(string userId)
    {
        ApplicationUser user = await GetUserAsync(userId);

        await _userManager.DeleteAsync(user);
    }

    private async Task<ApplicationUser> GetUserAsync(string userId)
    {
        ApplicationUser? user = await _userManager.Users
            .Include(x => x.Package)
            .Include(x => x.PackageConsumption)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null)
        {
            throw new UserException(ErrorCode.UserNotFound, HttpStatusCode.NotFound, $"Current user not found.");
        }

        return user;
    }
}