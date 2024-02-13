using System.Net.Http.Json;
using Reupload.Client.ApiServices.Contracts;
using Reupload.Client.ViewModels;
using Reupload.Client.ViewModels.Packages;

namespace Reupload.Client.ApiServices;

public class PackageApiService : IPackageApiService
{
    private readonly HttpClient _httpClient;

    public PackageApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Reupload.ServerAPI");
    }

    public async Task<ResultResponseVm<IEnumerable<PackageDetailsVm>>> GetAllDetailsAsync()
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/packages");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponseVm = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new ResultResponseVm<IEnumerable<PackageDetailsVm>>
            {
                Success = false,
                Errors = errorResponseVm!.Errors
            };
        }

        List<PackageDetailsVm>? packageDetailsVms = await httpResponseMessage.Content
            .ReadFromJsonAsync<List<PackageDetailsVm>>();

        return new ResultResponseVm<IEnumerable<PackageDetailsVm>>
        {
            Success = true,
            ObjectResult = packageDetailsVms!
        };
    }
}