using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Dtos.Packages;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;
using Reupload.Server.Services.Contracts;

namespace Reupload.Server.Services;

public class PackageService : IPackageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public PackageService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<IEnumerable<PackageDetailsDto>> GetAllDetailsAsync()
    {
        List<Package> packages = await _unitOfWork.Packages.FindAsync();

        return packages.Select(PackageDetailsDto.FromPackage);
    }

    public async Task IncrementConsumptionAsync(string userId)
    {
        ApplicationUser user = await GetUserAsync(userId);

        if (user.Package.Name != "Gold")
        {
            if (user.PackageConsumption.PhotoUploadAmount < user.Package.PhotoUploadLimit)
            {
                user.PackageConsumption.PhotoUploadAmount += 1;
                return;
            }

            throw new PackageException(ErrorCode.PackageReachedPhotoUploadLimit, HttpStatusCode.Locked,
                "You have reached photo upload limit, please consider upgrading.");
        }
    }

    public async Task DecrementConsumptionAsync(string userId)
    {
        ApplicationUser user = await GetUserAsync(userId);

        user.PackageConsumption.PhotoUploadAmount -= 1;
    }

    private async Task<ApplicationUser> GetUserAsync(string userId)
    {
        ApplicationUser? user = await _userManager.Users
            .Include(x => x.Package)
            .Include(x => x.PackageConsumption)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null)
        {
            throw new UserException(ErrorCode.UserNotFound, HttpStatusCode.NotFound, $"User with ID: {userId} does not exist.");
        }

        return user;
    }
}