using MediatR;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;


namespace WalletApp.Application.Feature.Handler;

public class TransferCommandHandler : IRequestHandler<TransferCommand, ServiceResponse<TransactionResponseDTO>>

{
    public readonly WalletService _walletService;

    public TransferCommandHandler(WalletService walletService)
    {
        _walletService = walletService;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _walletService.TransferAsync(request.SourceWalletId, request.TargetWalletId, request.Amount);

        if (transaction == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Transfer failed.");

        return ServiceResponse<TransactionResponseDTO>.Ok(new TransactionResponseDTO
        {
            WalletId = request.SourceWalletId,
            Amount = request.Amount,
            Type = transaction.Type,
            Description = transaction.Description
        }, "Transfer successful.");
    }
}
