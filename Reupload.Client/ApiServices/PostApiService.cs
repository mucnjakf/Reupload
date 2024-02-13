using System.Net.Http.Json;
using System.Text.Json;
using Reupload.Client.ApiServices.Contracts;
using Reupload.Client.ViewModels;
using Reupload.Client.ViewModels.Posts;
using Reupload.Client.ViewModels.Users;
using Reupload.Shared.Pagination;

namespace Reupload.Client.ApiServices;

public class PostApiService : IPostApiService
{
    private readonly HttpClient _publicHttpClient;
    private readonly HttpClient _httpClient;

    public PostApiService(IHttpClientFactory httpClientFactory)
    {
        _publicHttpClient = httpClientFactory.CreateClient("Reupload.Server");
        _httpClient = httpClientFactory.CreateClient("Reupload.ServerAPI");
    }

    public async Task<ResultResponseVm<PaginationResponseDto<PostDetailsVm>>> GetPaginatedDetailsAsync(
        PaginationRequestDto paginationRequestDto)
    {
        string queryString =
            $"pageNumber={paginationRequestDto.PageNumber}&pageSize={paginationRequestDto.PageSize}&searchQuery={paginationRequestDto.SearchQuery}";

        HttpResponseMessage httpResponseMessage =
            await _publicHttpClient.GetAsync($"api/posts?{queryString}");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponseVm = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new ResultResponseVm<PaginationResponseDto<PostDetailsVm>>
            {
                Success = false,
                Errors = errorResponseVm!.Errors
            };
        }

        List<PostDetailsVm>? postDetailsVms = await httpResponseMessage.Content
            .ReadFromJsonAsync<List<PostDetailsVm>>();

        PaginationMetadata? metadata = JsonSerializer.Deserialize<PaginationMetadata>(
            httpResponseMessage.Headers.GetValues("X-Pagination").First());

        PaginationResponseDto<PostDetailsVm> result = new()
        {
            Items = postDetailsVms!,
            Metadata = metadata!
        };

        return new ResultResponseVm<PaginationResponseDto<PostDetailsVm>>
        {
            Success = true,
            ObjectResult = result
        };
    }

    public async Task<Tuple<Stream, string>?> GetPostImageAsync(string userId, Guid postId)
    {
        HttpResponseMessage httpResponseMessage =
            await _publicHttpClient.GetAsync($"api/posts/{userId}/{postId}/image");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return null;
        }

        Stream imageStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        string contentType = httpResponseMessage.Content.Headers.GetValues("Content-Type").First();

        return new Tuple<Stream, string>(imageStream, contentType);
    }

    public async Task<ResultResponseVm<PaginationResponseDto<UserPostVm>>> GetPaginatedForUserDetailsAsync(
        PaginationRequestDto paginationRequestDto, string? userId)
    {
        string queryString =
            $"pageNumber={paginationRequestDto.PageNumber}&pageSize={paginationRequestDto.PageSize}";

        HttpResponseMessage httpResponseMessage =
            await _httpClient.GetAsync($"api/posts/user/{userId}?{queryString}");

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseVm? errorResponseVm = await httpResponseMessage.Content
                .ReadFromJsonAsync<ErrorResponseVm>();

            return new ResultResponseVm<PaginationResponseDto<UserPostVm>>
            {
                Success = false,
                Errors = errorResponseVm!.Errors
            };
        }

        List<UserPostVm>? userPostVms = await httpResponseMessage.Content
            .ReadFromJsonAsync<List<UserPostVm>>();

        PaginationMetadata? metadata = JsonSerializer.Deserialize<PaginationMetadata>(
            httpResponseMessage.Headers.GetValues("X-Pagination").First());

        PaginationResponseDto<UserPostVm> result = new()
        {
            Items = userPostVms!,
            Metadata = metadata!
        };

        return new ResultResponseVm<PaginationResponseDto<UserPostVm>>
        {
            Success = true,
            ObjectResult = result
        };
    }

    public async Task<EmptyResponseVm> InsertAsync(PostInsertVm postInsertVm)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("api/posts", postInsertVm);

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

    public async Task<EmptyResponseVm> UpdateAsync(Guid postId, PostUpdateVm postUpdateVm)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync($"api/posts/{postId}", postUpdateVm);

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

    public async Task<EmptyResponseVm> DeleteAsync(Guid postId)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"api/posts/{postId}");

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