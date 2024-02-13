using Azure.Storage.Blobs;

namespace Reupload.Server.AzureStorage.Blobs;

public interface IBlobStorageFactory
{
    BlobServiceClient CreateBlobServiceClient();
}