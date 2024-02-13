// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Dtos;
using Reupload.Server.Dtos.Packages;
using Reupload.Server.Dtos.UserActions;
using Reupload.Server.Enums;
using Reupload.Server.Models;
using Reupload.Server.Services.Contracts;
using Reupload.Shared.Constants;

namespace Reupload.Server.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IEmailSender _emailSender;
        private readonly IPackageService _packageService;
        private readonly IUserActionService _userActionService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IEmailSender emailSender,
            IPackageService packageService,
            ILogger<RegisterModel> logger,
            IUserActionService userActionService,
            IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailSender = emailSender;
            _packageService = packageService;

            _emailStore = GetEmailStore();

            _logger = logger;
            _userActionService = userActionService;
            _unitOfWork = unitOfWork;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public List<SelectListItem> Packages { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Email is required.")]
            [Display(Name = "Email")]
            [EmailAddress(ErrorMessage = "Email is not valid.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "First name is required.")]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required.")]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Package is required.")]
            [Display(Name = "Package")]
            public Guid PackageId { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            await GetPackagesSelectListAsync();
        }

        private async Task GetPackagesSelectListAsync()
        {
            IEnumerable<PackageDetailsDto> packages = await _packageService.GetAllDetailsAsync();

            foreach (PackageDetailsDto package in packages)
            {
                Packages.Add(new SelectListItem($"{package.Name} - {package.Price} EUR", package.Id.ToString()));
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                ApplicationUser user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.PackageId = Input.PackageId;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                IdentityResult result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    string userId = await _userManager.GetUserIdAsync(user);

                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    string callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId, code, returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");

                    await _userManager.AddToRoleAsync(user, Roles.User);

                    await AddPackageConsumptionAsync(userId, Input.PackageId);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }

                    await _userActionService.InsertAsync(new UserActionInsertDto
                    {
                        UserId = user.Id,
                        Timestamp = DateTime.Now,
                        ActionType = ActionType.Register,
                        ActionParametersJson = JsonSerializer.Serialize(new RegisterUserActionDto
                        {
                            Username = Input.Email,
                            FirstName = Input.FirstName,
                            LastName = Input.LastName,
                            PackageId = Input.PackageId
                        })
                    });

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                    await GetPackagesSelectListAsync();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task AddPackageConsumptionAsync(string userId, Guid packageId)
        {
            PackageConsumption packageConsumption = new()
            {
                PackageId = packageId,
                UserId = userId
            };

            await _unitOfWork.PackageConsumptions.AddAsync(packageConsumption);
            await _unitOfWork.SaveChangesAsync();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                                                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}