namespace minimal_api.Dominio.DTOs;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }

    public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        HasPrevious = pageNumber > 1;
        HasNext = pageNumber < TotalPages;
    }
}

