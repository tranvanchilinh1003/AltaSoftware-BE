public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; } // Đây chính là TotalCount mà bạn đang gọi
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public PagedResult(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = totalItems; // Đảm bảo bạn dùng đúng tên
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}
