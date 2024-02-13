using Reupload.Client.ViewModels;
using Reupload.Client.ViewModels.Packages;

namespace Reupload.Client.ApiServices.Contracts;

public interface IPackageApiService
{
    Task<ResultResponseVm<IEnumerable<PackageDetailsVm>>> GetAllDetailsAsync();
}