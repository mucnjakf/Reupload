namespace Reupload.Client.ViewModels;

public class EmptyResponseVm
{
    public bool Success { get; set; }

    public List<string> Errors { get; set; } = default!;
}