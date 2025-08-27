using MediatR;
using WalletApp.Application.Common.Pagination;

namespace WalletApp.Application.Abstraction.Services.PageBase
{
    public abstract class PagedSearchQuery<T> : IRequest<PagedResult<T>>
    {
        // Request tarafı
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }

}
