namespace Reupload.Shared.Pagination;

public class PagingLink
{
    public string Text { get; }

    public int Page { get; }

    public bool Enabled { get; }

    public bool Active { get; set; }

    public PagingLink(int page, bool enabled, string text)
    {
        Page = page;
        Enabled = enabled;
        Text = text;
    }
}