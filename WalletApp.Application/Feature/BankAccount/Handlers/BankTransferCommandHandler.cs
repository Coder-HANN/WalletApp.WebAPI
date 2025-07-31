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
        var currentUserId = _currentUser.CurrentUser();
        ///TODO : User null kontrolü
        // 1. IBAN'dan hedef banka kodunu al
        if (string.IsNullOrWhiteSpace(dto.Iban) || dto.Iban.Length < 8)
            return ServiceResponse<TransactionResponseDTO>.Fail("Geçersiz IBAN.");

        string targetBankCode = dto.Iban.Substring(4, 5);

        // 2. Kullanıcının tüm banka hesaplarını getir
        var userAccounts = await _bankAccountRepository.GetUserAccountsAsync(currentUserId);
        if (userAccounts == null || !userAccounts.Any())
            return ServiceResponse<TransactionResponseDTO>.Fail("Kullanıcının tanımlı banka hesabı yok.");

        // 3. Kaynak hesabı seç
        var sourceAccount = SelectSourceAccount(userAccounts, targetBankCode);
        if (sourceAccount == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Uygun kaynak banka hesabı bulunamadı.");

        // 4. Bakiye kontrolü
        if (sourceAccount.Balance < dto.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Yetersiz bakiye.");

        // 5. Alıcı banka hesabı
        var targetBank = await _bankAccountRepository.GetByIdAsync(dto.TargetBankId);
        if (targetBank == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Alıcı banka hesabı bulunamadı.");

        // 6. ProviderBank kontrolü
        if (string.IsNullOrWhiteSpace(dto.BankName))
            return ServiceResponse<TransactionResponseDTO>.Fail("BankName alanı zorunludur.");

        var providerBank = await _providerBankRepository.GetAsync(p => p.BankName == dto.BankName);
        if (providerBank == null)
        {
            providerBank = new ProviderBank { BankName = dto.BankName };
            await _providerBankRepository.AddAsync(providerBank);
            await _providerBankRepository.SaveChangesAsync();
        }

        // 7. Bakiye güncellemeleri
        sourceAccount.Balance -= dto.Amount;
        targetBank.Balance += dto.Amount;

        await _bankAccountRepository.UpdateAsync(sourceAccount);
        await _bankAccountRepository.UpdateAsync(targetBank);
        await _bankAccountRepository.SaveChangesAsync();

        // 8. Transaction oluştur
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

        // 9. BankTransaction oluştur
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

        // 10. Dönüş DTO’su
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
