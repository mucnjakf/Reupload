namespace Reupload.Shared.Pagination;

public class PaginationResponseDto<T> where T : class
{
    public List<T> Items { get; set; } = default!;

    public PaginationMetadata Metadata { get; set; } = default!;
}