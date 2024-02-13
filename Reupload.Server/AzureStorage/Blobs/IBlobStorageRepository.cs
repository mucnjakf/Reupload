namespace Reupload.Server.AzureStorage.Blobs;

public interface IBlobStorageRepository
{
    Task<string> GetBlobUriAsync(string containerName, string blobName);

    Task<Tuple<Stream, string>> GetBlobStreamAsync(string containerName, string blobName);

    Task<string> AddBlobAsync(string containerName, string blobName, Stream content, string contentType);

    Task DeleteBlobAsync(string containerName, string blobName);
}