using MediatR;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Application.Services.EntitiesRepositories;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Wallet.Handlers;

public class DepositCommandHandler : IRequestHandler<DepositRequestDTO, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DepositCommandHandler(IWalletRepository walletRepository, ITransactionRepository transactionRepository, IHttpContextAccessor httpContextAccessor) 
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(DepositRequestDTO request, CancellationToken cancellationToken)
    {
        // AppUserId çek
        var httpContext = _httpContextAccessor.HttpContext!;
        if (!httpContext.Items.TryGetValue("AppUserId", out var userIdObj) || userIdObj is not int appUserId)
        {
            return ServiceResponse<TransactionResponseDTO>.Fail("User ID not found in request context.");
        }

        // Validation
        if (request.Amount <= 0)
            return ServiceResponse<TransactionResponseDTO>.Fail("Amount must be greater than zero.");

        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
        if (wallet == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Wallet not found.");

        if (wallet.AppUserId != request.RequestedBy)
            return ServiceResponse<TransactionResponseDTO>.Fail("You do not own this wallet.");

        // İşlem
        var transaction = new Transaction
        {
            WalletId = request.WalletId,
            Amount = request.Amount,
            Type = TransactionType.Deposit,
            Description = request.Description ?? "Deposit"
        };

        await _transactionRepository.AddAsync(transaction);

        var dto = new TransactionResponseDTO
        {
            WalletId = transaction.WalletId,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description
        };

        return ServiceResponse<TransactionResponseDTO>.Ok(dto, "Deposit successful.");
    }
}
