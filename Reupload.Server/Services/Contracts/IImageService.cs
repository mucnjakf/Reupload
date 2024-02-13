namespace Reupload.Server.Services.Contracts;

public interface IImageService
{
    Task<string> GetPostImageUriAsync(string userId, Guid postId);

    Task<Tuple<Stream, string>> GetPostImageStreamAsync(string userId, Guid postId);

    Task InsertPostImageAsync(string userId, Guid postId, string base64Image, string contentType);

    Task DeletePostImageAsync(string userId, Guid postId);
}