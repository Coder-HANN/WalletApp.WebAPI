using MediatR;
using WalletApp.Domain.Enums;
using WalletApp.Application.Feature.Wallet.Dtos;
using Microsoft.AspNetCore.Http;


namespace WalletApp.Application.Feature.Wallet.Handlers;

public class WithdrawCommandHandler : IRequestHandler<WithdrawRequestDTO, ServiceResponse<TransactionResponseDTO>>
{
    private readonly WalletService _walletService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WithdrawCommandHandler(WalletService walletService, IHttpContextAccessor httpContextAccessor)
    {
        _walletService = walletService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(WithdrawRequestDTO request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        if (!httpContext.Items.TryGetValue("AppUserId", out var userIdObj) || userIdObj is not int appUserId)
        {
            return ServiceResponse<TransactionResponseDTO>.Fail("User ID not found in request context.");
        }

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
