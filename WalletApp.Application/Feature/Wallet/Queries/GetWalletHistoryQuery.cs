using MediatR;
using WalletApp.Application.DTO;


namespace WalletApp.Application.Features.Wallet.Queries.GetWalletHistory;

public record GetWalletHistoryQuery(Guid WalletId)
        : IRequest<IEnumerable<TransactionRequestDTO>>;
