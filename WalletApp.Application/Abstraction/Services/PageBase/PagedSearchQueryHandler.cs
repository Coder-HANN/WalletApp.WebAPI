using MediatR;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Common.Pagination
{
        public abstract class PagedSearchQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>where TRequest : IRequest<TResponse>
        {
            public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

            protected ServiceResponse<IEnumerable<T>> HandleResult<T>(IPagingExecutionResult<T> paginationResult)
            {
                return paginationResult.HasPaging
                    ? Infrastructure.Services.Pagenation.PaginatedResult<T>.Success(paginationResult.Data,
                    paginationResult.TotalCount,
                    paginationResult.CurrentPage,
                    paginationResult.PageSize)
                    : ServiceResponse<IEnumerable<T>>.Ok(data: paginationResult.Data);
            }
        }
}
