using X.PagedList.EntityFramework;

namespace WalletApp.Application.Common.Pagination
{
    public static class PaginationService
    {
        public static async Task<PagedResult<T>> CreatePagedResultAsync<T>(IQueryable<T> source,int? page,int? pageSize)
        {
            int currentPage = page ?? 1;
            int currentPageSize = pageSize ?? 10;

            // X.PagedList ile sayfalama yap
            var pagedList = await source.ToPagedListAsync(currentPage, currentPageSize);

            // PagedResult<T> olarak döndür
            return PagedResult<T>.Success(
                pagedList.ToList(),
                pagedList.TotalItemCount,
                pagedList.PageNumber,
                pagedList.PageSize
            );
        }
    }
}
