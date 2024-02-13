namespace Reupload.Server.Models;

public class Package
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public double Price { get; set; }

    public int PhotoUploadLimit { get; set; }
        
    public IEnumerable<ApplicationUser>? ApplicationUsers { get; set; }

    public IEnumerable<PackageConsumption>? PackageConsumptions { get; set; }
}