using MediatR;
using WalletApp.Domain.Enums;
using WalletApp.Application.Services;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Feature.Handler;

namespace WalletApp.Application.Feature.Handler;

public class DepositCommandHandler: IRequestHandler<DepositCommand, TransactionRequestDTO>
{
    private readonly WalletService _walletService;

    public DepositCommandHandler(WalletService walletService) => _walletService = walletService;

    public async Task<TransactionRequestDTO> Handle(DepositCommand cmd, CancellationToken ct)
    {
        var transaction= await _walletService.ProcessWalletTransactionAsync(cmd.WalletId,cmd.Amount,TransactionType.Deposit,cmd.Description);

        return transaction.ToString() == "null" ? null : new TransactionRequestDTO
        {
            WalletId = cmd.WalletId,
            Amount = cmd.Amount,
            Type = TransactionType.Deposit,
            Description = cmd.Description
        };
    }
}
