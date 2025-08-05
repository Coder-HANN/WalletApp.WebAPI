using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Enums;


namespace WalletApp.Application.Feature.Wallet.Handlers;

public class TransferCommandHandler : IRequestHandler<TransferRequestDTO, ServiceResponse<TransactionResponseDTO>>

{
    public readonly WalletService _walletService;
    public readonly IHttpContextAccessor _httpContextAccessor;
    public readonly ICurrentUserService _currentUserService;

    public TransferCommandHandler(
        WalletService walletService, 
        IHttpContextAccessor httpContextAccessor,
        ICurrentUserService currentUserService)
    {
        _walletService = walletService;
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(TransferRequestDTO request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Kullanıcı doğrualanamadı");
            var transaction = await _walletService.TransferAsync(request.SourceWalletId, request.TargetWalletId, request.Amount, request.Description);

        if (transaction == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Transfer failed.");

        return ServiceResponse<TransactionResponseDTO>.Ok(new TransactionResponseDTO
        {
            WalletId = request.SourceWalletId,
            Amount = request.Amount,
            Type = TransactionType.Transfer,
        }, "Transfer successful.");
    }
}
