namespace WalletApp.Application.Common.Pagination
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get;  set; }
        public int TotalCount { get;  set; }
        public int CurrentPage { get;  set; }
        public int PageSize { get;  set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);

        public PagedResult(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public static PagedResult<T> Success(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
        {
            return new PagedResult<T>(data, totalCount, currentPage, pageSize);
        }
    }
}
