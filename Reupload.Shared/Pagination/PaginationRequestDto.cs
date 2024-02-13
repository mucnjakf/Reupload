namespace Reupload.Shared.Pagination;

public class PaginationRequestDto
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 6;
    
    public string? SearchQuery { get; set; }
}