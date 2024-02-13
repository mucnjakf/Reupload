namespace Reupload.Server.Options;

public class AzureStorageOptions
{
    public const string SectionName = "AzureStorageOptions";

    public string ConnectionString { get; set; } = default!;
}