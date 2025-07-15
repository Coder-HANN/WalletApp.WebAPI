using MediatR;
using WalletApp.Application.Feature.DTO;


namespace WalletApp.Application.Feature.Queries;

public record GetWalletHistoryQuery(Guid WalletId)
        : IRequest<IEnumerable<TransactionRequestDTO>>;
