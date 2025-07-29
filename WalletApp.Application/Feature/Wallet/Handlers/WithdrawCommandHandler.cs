using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WalletApp.Application.Feature.Wallet.Handlers;

public class WithdrawCommandHandler : IRequestHandler<WithdrawRequestDTO, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WithdrawCommandHandler(
        IWalletRepository walletRepository,
        ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(WithdrawRequestDTO request, CancellationToken cancellationToken)
    {
        var appUserIdObj = _httpContextAccessor.HttpContext?.Items["AppUserId"];
        if (appUserIdObj == null || !int.TryParse(appUserIdObj.ToString(), out int parsedUserId))
            return ServiceResponse<TransactionResponseDTO>.Fail("User not authenticated.");


        var wallet = await _walletRepository.GetAsync(w => w.Id == request.WalletId && w.AppUserId == parsedUserId);
        if (wallet == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Wallet not found.");

        if (wallet.TotalBalance < request.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Insufficient balance.");

        var bankAccount = await _bankAccountRepository.GetAsync(b => b.Id == request.AppBankAccountId && b.AppUserId == parsedUserId);

        if (bankAccount == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Bank account not found.");

        // Bakiyeleri güncelle
        wallet.TotalBalance -= request.Amount;
        bankAccount.Balance += request.Amount;

        await _walletRepository.UpdateAsync(wallet);
        await _bankAccountRepository.UpdateAsync(bankAccount);

        // Transaction oluştur
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            WalletId = wallet.Id,
            Amount = -request.Amount, // Çekilen tutar negatif olmalı
            Type = TransactionType.Withdraw,
            Description = request.Description ?? $"Withdraw to bank account {bankAccount.Iban}",
            CreatedDate = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction);

        // Dönüş DTO'su
        var responseDto = new TransactionResponseDTO
        {
            Id = transaction.Id,
            WalletId = transaction.WalletId,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            CreatedDate = transaction.CreatedDate
        };

        return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, "Withdraw successful.");
    }
}
