using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Models;

namespace Reupload.Server.Data.Repositories;

public class PackageConsumptionRepository : Repository<ApplicationDbContext, PackageConsumption>, IPackageConsumptionRepository
{
    public PackageConsumptionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}