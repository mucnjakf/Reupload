using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Reupload.Server.Options;

namespace Reupload.Server.AzureStorage.Blobs;

public class BlobStorageFactory : IBlobStorageFactory
{
    private readonly AzureStorageOptions _azureStorageOptions;

    public BlobStorageFactory(IOptions<AzureStorageOptions> azureStorageOptions)
    {
        _azureStorageOptions = azureStorageOptions.Value;
    }

    public BlobServiceClient CreateBlobServiceClient()
    {
        BlobClientOptions blobClientOptions = new()
        {
            Retry = { Mode = RetryMode.Exponential }
        };

        return new BlobServiceClient(_azureStorageOptions.ConnectionString, blobClientOptions);
    }
}