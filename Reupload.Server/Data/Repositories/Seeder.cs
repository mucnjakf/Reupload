using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Models;
using Reupload.Server.Options;
using Reupload.Shared.Constants;

namespace Reupload.Server.Data.Repositories;

public class Seeder : ISeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AdminOptions _adminOptions;

    public Seeder(
        UserManager<ApplicationUser> userManager,
        IOptions<AdminOptions> adminOptions)
    {
        _userManager = userManager;
        _adminOptions = adminOptions.Value;
    }

    public async Task SeedAdminAsync()
    {
        ApplicationUser? applicationUser = await _userManager.FindByNameAsync(_adminOptions.Username);

        if (applicationUser is not null)
        {
            return;
        }

        applicationUser = new ApplicationUser
        {
            UserName = _adminOptions.Username,
            NormalizedUserName = _adminOptions.Username.ToUpper(),
            Email = _adminOptions.Username,
            NormalizedEmail = _adminOptions.Username.ToUpper(),
            FirstName = "Sys",
            LastName = "Admin",
            PackageId = Guid.Parse("CF47D685-F74A-428A-A5F9-583085973958"),
        };

        await _userManager.CreateAsync(applicationUser, _adminOptions.Password);

        await _userManager.AddToRoleAsync(applicationUser, Roles.Admin);
    }
}