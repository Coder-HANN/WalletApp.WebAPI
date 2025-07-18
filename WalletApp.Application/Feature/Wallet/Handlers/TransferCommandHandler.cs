using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using Microsoft.AspNetCore.Http;


namespace WalletApp.Application.Feature.Wallet.Handlers;

public class TransferCommandHandler : IRequestHandler<TransferRequestDTO, ServiceResponse<TransactionResponseDTO>>

{
    public readonly WalletService _walletService;
    public readonly IHttpContextAccessor _httpContextAccessor;

    public TransferCommandHandler(WalletService walletService, IHttpContextAccessor httpContextAccessor)
    {
        _walletService = walletService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(TransferRequestDTO request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("AppUserId");
        if (userIdClaim == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Unauthorized access.");

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
