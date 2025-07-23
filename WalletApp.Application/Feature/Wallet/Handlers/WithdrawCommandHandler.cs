using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Enums;


namespace WalletApp.Application.Feature.Wallet.Handlers;

public class WithdrawCommandHandler : IRequestHandler<WithdrawRequestDTO, ServiceResponse<IList<TransactionResponseDTO>>>
{
    private readonly WalletService _walletService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WithdrawCommandHandler(WalletService walletService, IHttpContextAccessor httpContextAccessor)
    {
        _walletService = walletService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<IList<TransactionResponseDTO>>> Handle(WithdrawRequestDTO request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        if (!httpContext.Items.TryGetValue("AppUserId", out var userIdObj) || userIdObj is not int appUserId)
        {
            return ServiceResponse<IList<TransactionResponseDTO>>.Fail("User ID not found in request context.");
        }
        

        var transaction = await _walletService.ProcessWalletTransactionAsync(
            request.WalletId,
            new TransferRequestDTO
            {
                Amount = request.Amount,
                Type = request.Type,
                Description = request.Description
            });

        if (transaction == null)
            return ServiceResponse<IList<TransactionResponseDTO>>.Fail("Withdraw failed.");


        return ServiceResponse<IList<TransactionResponseDTO>>.Ok(transaction, "Withdraw successful.");
    }
}
