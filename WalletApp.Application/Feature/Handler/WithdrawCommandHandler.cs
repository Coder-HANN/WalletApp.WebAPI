using MediatR;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Services;
using WalletApp.Domain.Enums;


namespace WalletApp.Application.Feature.Handler;

public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, ServiceResponse<TransactionResponseDTO>>
{
    private readonly WalletService _walletService;

    public WithdrawCommandHandler(WalletService walletService)
    {
        _walletService = walletService;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        // Validation (şimdilik atlıyoruz)

        var transaction = await _walletService.ProcessWalletTransactionAsync(
            request.WalletId, request.Amount, TransactionType.Withdraw, request.Description);

        if (transaction == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Withdraw failed.");

        var dto = new TransactionResponseDTO
        {
            WalletId = transaction.WalletId,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description
        };

        return ServiceResponse<TransactionResponseDTO>.Ok(dto, "Withdraw successful.");
    }
}
