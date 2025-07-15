using MediatR;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Feature.Queries;
using WalletApp.Application.Services;

namespace WalletApp.Application.Feature.Handler;

public class GetWalletHistoryQueryHandler: IRequestHandler<GetWalletHistoryQuery, IEnumerable<TransactionRequestDTO>>
{
    private readonly WalletService _walletService;

    public GetWalletHistoryQueryHandler(WalletService walletService)  => _walletService = walletService;

    public async Task<IEnumerable<TransactionRequestDTO>> Handle(GetWalletHistoryQuery q, CancellationToken ct)
    {
        var transactions = await _walletService.GetTransactionHistoryAsync(q.WalletId);

        return transactions.Select(t => new TransactionRequestDTO
        {
            WalletId = t.WalletId,
            Amount = t.Amount,
            Type = t.Type,
            Description = t.Description,
        });

    }
}
