namespace Reupload.Shared.Pagination;

public class PagedList<T> : List<T>
{
    public PaginationMetadata Metadata { get; }

    public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Metadata = new PaginationMetadata
        {
            TotalCount = totalCount,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        AddRange(items);
    }
}