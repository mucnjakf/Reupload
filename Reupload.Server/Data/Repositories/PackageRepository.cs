using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Models;

namespace Reupload.Server.Data.Repositories;

public class PackageRepository : Repository<ApplicationDbContext, Package>, IPackageRepository
{
    public PackageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> AnyAsync(Expression<Func<Package, bool>> expression)
    {
        IQueryable<Package> query = DbContext.Set<Package>();

        return await query.AnyAsync(expression);
    }
}