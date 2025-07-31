using MediatR;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;

public class BankTransferCommandHandler : IRequestHandler<BankTransferRequestDTO, ServiceResponse<TransactionResponseDTO>>
{
    private const string VakifBankCode = "0015";
    private const string ZiraatBankCode = "0010";
    private const string GarantiBankCode = "0020";

    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly ICurrentUserService _currentUser;

    public BankTransferCommandHandler(
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository,
        IProviderBankRepository providerBankRepository,
        IBankTransactionRepository bankTransactionRepository,
        ICurrentUserService currentUser)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _providerBankRepository = providerBankRepository;
        _bankTransactionRepository = bankTransactionRepository;
        _currentUser = currentUser;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(BankTransferRequestDTO dto, CancellationToken cancellationToken)
    {
        // 1. Kullanıcı ID'sini alıyoruz
        var currentUserId = _currentUser.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Kullanıcı doğrulanamadı.");

        // 2. IBAN geçerlilik kontrolü yapıyoruz
        if (string.IsNullOrWhiteSpace(dto.Iban) || dto.Iban.Length < 8)
            return ServiceResponse<TransactionResponseDTO>.Fail("Geçersiz IBAN.");

        // 3. IBAN'dan boşlukları kaldır ve uzunluk kontrolü
        string cleanedIban = dto.Iban.Replace(" ", "");
        if (cleanedIban.Length < 9)
            return ServiceResponse<TransactionResponseDTO>.Fail("Geçersiz IBAN.");

        // 4. IBAN içinden hedef bankanın kodunu alıyoruz 
        string targetBankCode = cleanedIban.Substring(4, 5);

        // 5. Kullanıcının kendi banka hesaplarını çekiyoruz
        var userAccounts = await _bankAccountRepository.GetUserAccountsAsync(currentUserId);
        if (userAccounts == null || !userAccounts.Any())
            return ServiceResponse<TransactionResponseDTO>.Fail("Kullanıcının tanımlı banka hesabı yok.");

        // 6. Kullanıcının hesapları içinden uygun kaynak hesabı seçiyoruz
        var sourceAccount = SelectSourceAccount(userAccounts, targetBankCode);
        if (sourceAccount == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Uygun kaynak banka hesabı bulunamadı.");

        // 7. Kaynak hesapta yeterli bakiye var mı kontrol ediyoruz
        if (sourceAccount.Balance < dto.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Yetersiz bakiye.");

        // 8. Tüm sistemde hedef IBAN'a sahip hesabı buluyoruz
        var targetBank = await _bankAccountRepository
            .GetAsync(x => x.Iban.Replace(" ", "") == cleanedIban);

        if (targetBank == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Hedef banka hesabı bulunamadı.");

        // 9. Hedef bankanın bilgilerini alıyoruz
        var providerBank = await _providerBankRepository.GetAsync(p => p.Id == targetBank.ProviderBankId);
        if (providerBank == null)
        {
            // Banka bilgisi yoksa yeni kayıt oluşturuyoruz
            providerBank = new ProviderBank { BankName = "Bilinmeyen Banka" };
            await _providerBankRepository.AddAsync(providerBank);
            await _providerBankRepository.SaveChangesAsync();
        }

        // 10. Kaynak hesaptan parayı düş, hedef hesaba ekle
        sourceAccount.Balance -= dto.Amount;
        targetBank.Balance += dto.Amount;

        await _bankAccountRepository.UpdateAsync(sourceAccount);
        await _bankAccountRepository.UpdateAsync(targetBank);
        await _bankAccountRepository.SaveChangesAsync();

        // 11. İşlem kaydını oluşturuyoruz
        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Type = TransactionType.BankTransfer,
            Currency = 0,
            Description = dto.Description ?? $"Banka transferi - {dto.Iban}",
            CreatedDate = DateTime.UtcNow
        };
        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();

        // 12. Banka işlem detaylarını kaydediyoruz
        var bankTransaction = new BankTransaction
        {
            TransactionId = transaction.Id,
            ProviderBankId = providerBank.Id,
            Iban = dto.Iban,
            TargetBankId = targetBank.Id,
            SourceBankId = sourceAccount.ProviderBankId,
            Commission = "0"
        };
        await _bankTransactionRepository.AddAsync(bankTransaction);
        await _bankTransactionRepository.SaveChangesAsync();

        // 13. Sonuç olarak başarılı mesaj ve işlem detaylarını döndürüyoruz
        var responseDto = new TransactionResponseDTO
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            CreatedDate = transaction.CreatedDate
        };

        return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, "Banka transferi başarıyla gerçekleştirildi.");
    }

    private AppBankAccount? SelectSourceAccount(IEnumerable<AppBankAccount> accounts, string targetBankCode)
    {
        // 1. Aynı bankadan varsa onu kullan
        var sameBank = accounts.FirstOrDefault(a => a.BankCode == targetBankCode);
        if (sameBank != null) return sameBank;

        // 2. Garanti ise Ziraat'tan gönder
        if (targetBankCode == GarantiBankCode)
            return accounts.FirstOrDefault(a => a.BankCode == ZiraatBankCode);

        // 3. Diğer durumlarda Vakıfbank'tan gönder
        return accounts.FirstOrDefault(a => a.BankCode == VakifBankCode);
    }
}
