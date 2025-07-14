using MediatR;
using WalletApp.Application.Command;
using WalletApp.Application.DTO;
using WalletApp.Application.Services;

namespace WalletApp.Application.Features.Wallet.Commands;

public class TransferCommandHandler
    : IRequestHandler<TransferCommand, TransactionRequestDTO>
{
    private readonly WalletService _walletService;

    public TransferCommandHandler(WalletService walletService)
    {
        _walletService = walletService;
    }

    public async Task<TransactionRequestDTO> Handle(TransferCommand cmd, CancellationToken ct)
    {
        var transaction = await _walletService.TransferAsync(cmd.SourceWalletId,cmd.TargetWalletId,cmd.Amount);
        return transaction.ToString() == "null" ? null : new TransactionRequestDTO
        {
            WalletId = cmd.SourceWalletId,
            Amount = cmd.Amount,
            Type = transaction.Type
        };

    }
}

