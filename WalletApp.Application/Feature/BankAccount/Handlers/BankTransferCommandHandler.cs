using MediatR;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;

public class BankTransferCommandHandler : IRequestHandler<BankTransferRequestDTO, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly ICurrentUserService _currentUser;

    public BankTransferCommandHandler(
        IWalletRepository walletRepository,
        IProviderBankRepository providerBankRepository,
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository,
        IBankTransactionRepository bankTransactionRepository,
        ICurrentUserService currentUser)
    {
        _walletRepository = walletRepository;
        _providerBankRepository = providerBankRepository;
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _bankTransactionRepository = bankTransactionRepository;
        _currentUser = currentUser;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(BankTransferRequestDTO dto, CancellationToken cancellationToken)
    {
        // Kullanıcı kimliği al
        var currentUserId = _currentUser.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Kullanıcı doğrulanamadı.");

        // Kullanıcı cüzdanını al
        var wallet = await _walletRepository.GetByUserIdAsync(currentUserId);
        if (wallet == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Kullanıcı cüzdanı bulunamadı.");

        // Cüzdan bakiyesi kontrolü
        if (wallet.TotalBalance < dto.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Cüzdan bakiyesi yetersiz.");

        // IBAN doğrulama
        if (string.IsNullOrWhiteSpace(dto.Iban) || dto.Iban.Replace(" ", "").Length < 26)
            return ServiceResponse<TransactionResponseDTO>.Fail("Geçersiz IBAN.");

        var cleanedIban = dto.Iban.Replace(" ", "");
        var targetBankCode = cleanedIban.Substring(5, 4);

        // Kullanıcının provider bank hesaplarını al
        var providerBanks = await _providerBankRepository.GetByUserIdAsync(currentUserId);
        if (providerBanks == null || !providerBanks.Any())
            return ServiceResponse<TransactionResponseDTO>.Fail("Kullanıcının provider banka hesabı yok.");

        // Hangi provider bankadan para göndereceğimizi seç
        var sourceProviderBank = SelectSourceProviderBank(providerBanks, targetBankCode);
        if (sourceProviderBank == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Uygun provider banka bulunamadı.");

        if (sourceProviderBank.TotalBalance < dto.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Provider banka bakiyesi yetersiz.");

        // Hedef banka hesabını bul
        var targetBankAccount = await _bankAccountRepository
            .GetAsync(b => b.Iban.Replace(" ", "") == cleanedIban);

        if (targetBankAccount == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Hedef banka hesabı bulunamadı.");

        // İşlem:  
        // 1. Cüzdan bakiyesi düşür  
        wallet.TotalBalance -= dto.Amount;
        await _walletRepository.UpdateAsync(wallet);

        // 2. Provider banka bakiyesi düşür  

        sourceProviderBank.TotalBalance -= dto.Amount;
        await _providerBankRepository.UpdateAsync(sourceProviderBank);

        // 3. Hedef banka bakiyesi artır (eğer hedef sistem içindeyse, yoksa dışa transfer olmuş sayılabilir)  
        targetBankAccount.Balance += dto.Amount;
        await _bankAccountRepository.UpdateAsync(targetBankAccount);

        // 4. Transaction kaydı oluştur  
        var transaction = new Transaction
        {
            WalletId = wallet.Id,
            Amount = dto.Amount,
            Type = TransactionType.BankTransfer,
            Description = dto.Description ?? $"Banka transferi - {dto.Iban}",
            CreatedDate = DateTime.UtcNow
        };
        await _transactionRepository.AddAsync(transaction);

        // 5. BankTransaction kaydı oluştur  
        var bankTransaction = new BankTransaction
        {
            TransactionId = transaction.Id,
            ProviderBankId = sourceProviderBank.Id,
            Iban = dto.Iban,
            TargetBankId = targetBankAccount.Id,
            Commission = "0"
        };
        await _bankTransactionRepository.AddAsync(bankTransaction);

        var responseDto = new TransactionResponseDTO
        {
            Id = transaction.Id,
            WalletId = wallet.Id,
            Amount = dto.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            CreatedDate = transaction.CreatedDate
        };

        return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, "Para transferi başarıyla gerçekleştirildi.");
    }

    private ProviderBank? SelectSourceProviderBank(IEnumerable<ProviderBank> accounts, string targetBankCode)
    {
        const string VakifBankCode = "0015";
        const string ZiraatBankCode = "0010";
        const string GarantiBankCode = "0020";

        // Aynı bankadan varsa onu kullan
        var sameBank = accounts.FirstOrDefault(a => a.BankCode == targetBankCode);
        if (sameBank != null) return sameBank;

        // Garanti ise Ziraat'tan gönder
        if (targetBankCode == GarantiBankCode)
            return accounts.FirstOrDefault(a => a.BankCode == ZiraatBankCode);

        // Diğer durumlarda Vakıfbank'tan gönder
        return accounts.FirstOrDefault(a => a.BankCode == VakifBankCode);
    }
}
