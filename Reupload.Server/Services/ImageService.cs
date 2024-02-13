using Reupload.Server.AzureStorage.Blobs;
using Reupload.Server.Services.Contracts;

namespace Reupload.Server.Services;

public class ImageService : IImageService
{
    private readonly IBlobStorageRepository _blobStorageRepository;

    public ImageService(IBlobStorageRepository blobStorageRepository)
    {
        _blobStorageRepository = blobStorageRepository;
    }

    public async Task<string> GetPostImageUriAsync(string userId, Guid postId)
    {
        return await _blobStorageRepository
            .GetBlobUriAsync("reupload-posts", $"{userId}/{postId.ToString().ToUpper()}");
    }

    public async Task<Tuple<Stream, string>> GetPostImageStreamAsync(string userId, Guid postId)
    {
        Tuple<Stream, string> imageStream = await _blobStorageRepository
            .GetBlobStreamAsync("reupload-posts", $"{userId}/{postId.ToString().ToUpper()}");

        return imageStream;
    }

    public async Task InsertPostImageAsync(string userId, Guid postId, string base64Image, string contentType)
    {
        byte[] buffer = Convert.FromBase64String(base64Image);
        MemoryStream imageStream = new(buffer);

        await _blobStorageRepository.AddBlobAsync("reupload-posts",
            $"{userId}/{postId.ToString().ToUpper()}", imageStream, contentType);
    }

    public async Task DeletePostImageAsync(string userId, Guid postId)
    {
        await _blobStorageRepository.DeleteBlobAsync("reupload-posts",
            $"{userId}/{postId.ToString().ToUpper()}");
    }
}