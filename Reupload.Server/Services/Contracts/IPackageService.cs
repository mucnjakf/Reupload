using Reupload.Server.Dtos;
using Reupload.Server.Dtos.Packages;

namespace Reupload.Server.Services.Contracts;

public interface IPackageService
{
    Task<IEnumerable<PackageDetailsDto>> GetAllDetailsAsync();

    Task IncrementConsumptionAsync(string userId);

    Task DecrementConsumptionAsync(string userId);
}