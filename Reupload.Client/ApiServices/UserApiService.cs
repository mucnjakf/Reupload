using System.Net.Http.Json;
using System.Text.Json;
using Reupload.Client.ApiServices.Contracts;
using Reupload.Client.ViewModels;
using Reupload.Client.ViewModels.Users;
using Reupload.Shared.Pagination;

namespace Reupload.Client.ApiServices;

public class UserApiService : IUserApiService
{
    private readonly HttpClient _httpClient;

    public UserApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Reupload.ServerAPI");
    }

    public async Task<ResultResponseVm<UserDetailsVm>> GetCurrentUserDetailsAsync()
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/users/current-user");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponseVm = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new ResultResponseVm<UserDetailsVm>()
            {
                Success = false,
                Errors = errorResponseVm!.Errors
            };
        }

        UserDetailsVm? applicationUserDetailsVm =
            await httpResponseMessage.Content.ReadFromJsonAsync<UserDetailsVm>();

        return new ResultResponseVm<UserDetailsVm>
        {
            Success = true,
            ObjectResult = applicationUserDetailsVm!
        };
    }

    public async Task<EmptyResponseVm> UpdateUserPackageAsync(UserChangePackageVm userChangePackageVm)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync("api/users/change-package", userChangePackageVm);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponseVm = await httpResponseMessage.Content.ReadFromJsonAsync<ErrorResponseVm>();

            return new EmptyResponseVm
            {
                Success = false,
                Errors = errorResponseVm!.Errors
            };
        }

        return new EmptyResponseVm { Success = true };
    }

    public async Task<ResultResponseVm<PaginationResponseDto<UserTableVm>>> GetPaginatedTableAsync(
        PaginationRequestDto paginationRequestDto)
    {
        string queryString =
            $"pageNumber={paginationRequestDto.PageNumber}&pageSize={paginationRequestDto.PageSize}";

        HttpResponseMessage httpResponseMessage =
            await _httpClient.GetAsync($"api/users/table?{queryString}");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponseVm = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new ResultResponseVm<PaginationResponseDto<UserTableVm>>
            {
                Success = false,
                Errors = errorResponseVm!.Errors
            };
        }

        List<UserTableVm>? userTableVms = await httpResponseMessage.Content
            .ReadFromJsonAsync<List<UserTableVm>>();

        PaginationMetadata? metadata = JsonSerializer.Deserialize<PaginationMetadata>(
            httpResponseMessage.Headers.GetValues("X-Pagination").First());

        PaginationResponseDto<UserTableVm> result = new()
        {
            Items = userTableVms!,
            Metadata = metadata!
        };

        return new ResultResponseVm<PaginationResponseDto<UserTableVm>>
        {
            Success = true,
            ObjectResult = result
        };
    }

    public async Task<ResultResponseVm<UserDetailsVm>> GetDetailsAsync(string userId)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"api/users/{userId}");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponse = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new ResultResponseVm<UserDetailsVm>
            {
                Success = false,
                Errors = errorResponse!.Errors
            };
        }

        UserDetailsVm? userDetailsVm = await httpResponseMessage.Content
            .ReadFromJsonAsync<UserDetailsVm>();

        return new ResultResponseVm<UserDetailsVm>
        {
            Success = true,
            ObjectResult = userDetailsVm!
        };
    }

    public async Task<EmptyResponseVm> UpdateAsync(string userId, UserUpdateVm userUpdateVm)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync($"api/users/{userId}", userUpdateVm);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponseVm = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new EmptyResponseVm
            {
                Success = false,
                Errors = errorResponseVm!.Errors
            };
        }

        return new EmptyResponseVm { Success = true };
    }
    
    public async Task<EmptyResponseVm> DeleteAsync(string userId)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"api/users/{userId}");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponseVm = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new EmptyResponseVm
            {
                Success = false,
                Errors = errorResponseVm!.Errors
            };
        }

        return new EmptyResponseVm { Success = true };
    }
}