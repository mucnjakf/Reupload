namespace Reupload.Client.ViewModels;

public class ResultResponseVm<T> where T : class
{
    public bool Success { get; set; }

    public List<string> Errors { get; set; } = default!;

    public T ObjectResult { get; set; } = default!;
}