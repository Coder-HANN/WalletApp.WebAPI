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
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DepositCommandHandler(
        IWalletRepository walletRepository,
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository,
        IBankTransactionRepository bankTransactionRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _walletRepository = walletRepository;
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _bankTransactionRepository = bankTransactionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(DepositRequestDTO request, CancellationToken cancellationToken)
    {
        // Kullanıcı doğrulaması
        if (!(_httpContextAccessor.HttpContext?.Items.TryGetValue("AppUserId", out var userIdObj) == true
            && int.TryParse(userIdObj?.ToString(), out var appUserId)))
        {
            return ServiceResponse<TransactionResponseDTO>.Fail("User ID not found in request context.");
        }

        // Banka hesabı kontrolü
        var bankAccount = await _bankAccountRepository.GetByIdAsync(request.SourceBankId);

        if (bankAccount == null || bankAccount.AppUserId != appUserId)
            return ServiceResponse<TransactionResponseDTO>.Fail("Invalid bank account.");

        // Tutar kontrolü
        if (request.Amount <= 0)
            return ServiceResponse<TransactionResponseDTO>.Fail("Amount must be greater than zero.");

        if (bankAccount.Balance < request.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Insufficient bank account balance.");

        // Cüzdan kontrolü
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);

        if (wallet == null || wallet.AppUserId != appUserId)
            return ServiceResponse<TransactionResponseDTO>.Fail("Wallet not found or not owned by user.");

        // Banka hesabından para düşülür
        bankAccount.Balance -= request.Amount;

        // Cüzdana para eklenir
        wallet.TotalBalance += request.Amount;

        // Veritabanı güncellemeleri
        await _bankAccountRepository.UpdateAsync(bankAccount);
        await _walletRepository.UpdateAsync(wallet);

        // Transaction nesnesi oluşturulur ve eklenir
        var transaction = new Transaction
        {
            WalletId = wallet.Id,
            Amount = request.Amount,
            Type = TransactionType.Deposit,
            Description = request.Description ?? "Deposit from bank account",
            CreatedDate = DateTime.UtcNow // Eğer otomatik atanıyorsa bu satır opsiyonel
        };
        await _transactionRepository.AddAsync(transaction);

        // BankTransaction nesnesi oluşturulur ve TransactionId bağlanır
        var bankTransaction = new BankTransaction
        {
            TransactionId = transaction.Id,
            ProviderBankId = bankAccount.ProviderBankId, // Banka sağlayıcı ID
            Iban = bankAccount.Iban,
            SourceBankId = bankAccount.Id, // 💡 EKLENDİ: Hangi kullanıcı hesabından geldiğini göster
            
            TargetBankId = Guid.Empty, // Cüzdan olduğu için boş bırakabilir veya nullable yapabilirsin
            Commission = "0", // Komisyon varsa ayarla, yoksa sıfır olarak bırak
            Transaction = transaction
        };
        await _bankTransactionRepository.AddAsync(bankTransaction);

        // DTO'yu oluştur
        var responseDto = new TransactionResponseDTO
        {
            Id = transaction.Id,
            WalletId = wallet.Id,
            Amount = request.Amount,
            Type = TransactionType.Deposit,
            Description = transaction.Description,
            CreatedDate = transaction.CreatedDate,
            Suggestion = "Deposit completed successfully"
        };

        return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, "Deposit successful.");
    }
}
