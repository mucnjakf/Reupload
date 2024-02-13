namespace Reupload.Shared.Helpers;

public class FileExtensionHelpers
{
    public static string GetFileExtension(string contentType)
    {
        return contentType switch
        {
            "image/bmp" => "bmp",
            "image/png" => "png",
            _ => "jpg"
        };
    }
}