using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;

public class WithdrawCommandHandler : IRequestHandler<WithdrawRequestDTO, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentUserService _currentUserService;

    public WithdrawCommandHandler(
        IWalletRepository walletRepository,
        ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository,
        IProviderBankRepository providerBankRepository,
        IBankTransactionRepository bankTransactionRepository,
        IHttpContextAccessor httpContextAccessor,
        ICurrentUserService currentUserService)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
        _providerBankRepository = providerBankRepository;
        _bankTransactionRepository = bankTransactionRepository;
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(WithdrawRequestDTO request, CancellationToken cancellationToken)
    {
        // Kullanıcı doğrulama
        var currentUserId = _currentUserService.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Kullanıcı doğrulanamadı.");
            // Cüzdan kontrol
            var wallet = await _walletRepository.GetAsync(w => w.Id == request.WalletId && w.AppUserId == currentUserId);
        if (wallet == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Wallet not found.");

        if (wallet.TotalBalance < request.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Insufficient wallet balance.");

        // Kullanıcının banka hesabı kontrol
        var bankAccount = await _bankAccountRepository.GetAsync(b => b.Id == request.AppBankAccountId && b.AppUserId == currentUserId);
        if (bankAccount == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Bank account not found.");

        // Provider bankayı bul (kullanıcının hesabı hangi sağlayıcıya bağlı?)
        var providerBank = await _providerBankRepository.GetByIdAsync(bankAccount.ProviderBankId);
        if (providerBank == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Provider bank not found.");

        if (providerBank.TotalBalance < request.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Provider bank balance is insufficient.");

        // Bakiye güncelle
        wallet.TotalBalance -= request.Amount; // cüzdandan düş
        providerBank.TotalBalance -= request.Amount; // provider bankadan düş
        bankAccount.Balance += request.Amount; // kullanıcının banka hesabına ekle

        await _walletRepository.UpdateAsync(wallet);
        await _providerBankRepository.UpdateAsync(providerBank);
        await _bankAccountRepository.UpdateAsync(bankAccount);

        // Transaction oluştur
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            WalletId = wallet.Id,
            Amount = -request.Amount,
            Type = TransactionType.Withdraw,
            Description = request.Description ?? $"Withdraw to bank account {bankAccount.Iban}",
            CreatedDate = DateTime.UtcNow
        };
        await _transactionRepository.AddAsync(transaction);

        // BankTransaction kaydı
        var bankTransaction = new BankTransaction
        {
            TransactionId = transaction.Id,
            ProviderBankId = providerBank.Id,
            SourceBankId = bankAccount.Id,     // Kullanıcının hesabı
            TargetBankId = bankAccount.Id,            // Para gönderilen hesap
            Iban = bankAccount.Iban,
            Commission = "0"
        };
        await _bankTransactionRepository.AddAsync(bankTransaction);

        // DTO dönüş
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
