using System.Linq.Expressions;
using Reupload.Server.Models;

namespace Reupload.Server.Data.Repositories.Contracts;

public interface IPackageRepository : IRepository<Package>
{
    Task<bool> AnyAsync(Expression<Func<Package, bool>> expression);
}