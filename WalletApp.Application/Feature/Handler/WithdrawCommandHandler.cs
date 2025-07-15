using MediatR;
using WalletApp.Application.Services;
using WalletApp.Domain.Enums;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Handler;

public class WithdrawCommandHandler
    : IRequestHandler<WithdrawCommand, TransactionRequestDTO>
{
    private readonly WalletService _walletService;

    public WithdrawCommandHandler(WalletService walletService)
    {
        _walletService = walletService;
    }

    public async Task<TransactionRequestDTO> Handle(WithdrawCommand cmd, CancellationToken ct)
    {
        var transaction = await _walletService.ProcessWalletTransactionAsync(cmd.WalletId,cmd.Amount,TransactionType.Withdraw,cmd.Description);

        return transaction.ToString() == "null" ? null : new TransactionRequestDTO
        {
            WalletId = cmd.WalletId,
            Amount = cmd.Amount,
            Type = TransactionType.Withdraw,
            Description = cmd.Description
        };
    }
}
