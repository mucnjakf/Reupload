using Microsoft.AspNetCore.Identity;

namespace Reupload.Server.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public IEnumerable<Post>? Posts { get; set; }

    public Guid PackageId { get; set; }

    public Package Package { get; set; } = default!;

    public bool PackageIsActive { get; set; } = true;

    public IEnumerable<UserAction>? UserActions { get; set; }
    
    public Guid PackageConsumptionId { get; set; }

    public PackageConsumption PackageConsumption { get; set; } = default!;
}