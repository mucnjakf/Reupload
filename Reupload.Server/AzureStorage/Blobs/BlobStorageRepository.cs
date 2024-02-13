using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Reupload.Server.Exceptions;
using Reupload.Shared.Helpers;

namespace Reupload.Server.AzureStorage.Blobs;

public class BlobStorageRepository : IBlobStorageRepository
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageRepository(IBlobStorageFactory blobStorageFactory)
    {
        _blobServiceClient = blobStorageFactory.CreateBlobServiceClient();
    }

    public async Task<string> GetBlobUriAsync(string containerName, string blobName)
    {
        BlobClient blobClient = await GetBlobClientAsync(containerName, blobName);

        if (!await blobClient.ExistsAsync())
        {
            throw new AzureStorageException();
        }

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<Tuple<Stream, string>> GetBlobStreamAsync(string containerName, string blobName)
    {
        BlobClient blobClient = await GetBlobClientAsync(containerName, blobName);

        if (!await blobClient.ExistsAsync())
        {
            throw new AzureStorageException();
        }

        BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
        string contentType = blobProperties.ContentType;

        MemoryStream content = new();
        await blobClient.DownloadToAsync(content);
        content.Position = 0;

        return new Tuple<Stream, string>(content, contentType);
    }

    public async Task<string> AddBlobAsync(string containerName, string blobName, Stream content, string contentType)
    {
        BlobClient blobClient = await GetBlobClientAsync(containerName, blobName);

        if (await blobClient.ExistsAsync())
        {
            throw new AzureStorageException();
        }

        content.Position = 0;

        string fileExtension = FileExtensionHelpers.GetFileExtension(contentType);

        BlobHttpHeaders blobHttpHeaders = new() { ContentType = fileExtension };

        await blobClient.UploadAsync(content, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });

        return blobClient.Uri.AbsolutePath;
    }

    public async Task DeleteBlobAsync(string containerName, string blobName)
    {
        BlobClient blobClient = await GetBlobClientAsync(containerName, blobName);

        if (!await blobClient.ExistsAsync())
        {
            throw new AzureStorageException();
        }

        await blobClient.DeleteAsync();
    }

    private async Task<BlobClient> GetBlobClientAsync(string containerName, string blobName)
    {
        BlobContainerClient? blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        if (!await blobContainerClient.ExistsAsync())
        {
            await blobContainerClient.CreateAsync();
        }

        return blobContainerClient.GetBlobClient(blobName);
    }
}