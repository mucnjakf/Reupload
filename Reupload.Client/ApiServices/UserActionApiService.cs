using System.Net.Http.Json;
using System.Text.Json;
using Reupload.Client.ApiServices.Contracts;
using Reupload.Client.ViewModels;
using Reupload.Client.ViewModels.UserActions;
using Reupload.Shared.Pagination;

namespace Reupload.Client.ApiServices;

public class UserActionApiService : IUserActionApiService
{
    private readonly HttpClient _httpClient;

    public UserActionApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Reupload.ServerAPI");
    }

    public async Task<ResultResponseVm<PaginationResponseDto<UserActionTableVm>>> GetPaginatedTableAsync(
        PaginationRequestDto paginationRequestDto)
    {
        string queryString =
            $"pageNumber={paginationRequestDto.PageNumber}&pageSize={paginationRequestDto.PageSize}";

        HttpResponseMessage httpResponseMessage =
            await _httpClient.GetAsync($"api/user-actions/table?{queryString}");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponseVm = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new ResultResponseVm<PaginationResponseDto<UserActionTableVm>>
            {
                Success = false,
                Errors = errorResponseVm!.Errors
            };
        }

        List<UserActionTableVm>? userActionTableVms = await httpResponseMessage.Content
            .ReadFromJsonAsync<List<UserActionTableVm>>();

        PaginationMetadata? metadata = JsonSerializer.Deserialize<PaginationMetadata>(
            httpResponseMessage.Headers.GetValues("X-Pagination").First());

        PaginationResponseDto<UserActionTableVm> result = new()
        {
            Items = userActionTableVms!,
            Metadata = metadata!
        };

        return new ResultResponseVm<PaginationResponseDto<UserActionTableVm>>
        {
            Success = true,
            ObjectResult = result
        };
    }

    public async Task<ResultResponseVm<UserActionDetailsVm>> GetDetailsAsync(Guid userActionId)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"api/user-actions/{userActionId}");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponse = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new ResultResponseVm<UserActionDetailsVm>
            {
                Success = false,
                Errors = errorResponse!.Errors
            };
        }

        UserActionDetailsVm? userActionDetailsVm = await httpResponseMessage.Content
            .ReadFromJsonAsync<UserActionDetailsVm>();

        return new ResultResponseVm<UserActionDetailsVm>
        {
            Success = true,
            ObjectResult = userActionDetailsVm!
        };
    }
}