using MediatR;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;

public class BankTransferCommandHandler : IRequestHandler<BankTransferRequestDTO, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankTransferCommandHandler(
        IBankTransactionRepository bankTransactionRepository,
        ITransactionRepository transactionRepository,
        IProviderBankRepository providerBankRepository,
        IBankAccountRepository bankAccountRepository)
    {
        _bankTransactionRepository = bankTransactionRepository;
        _transactionRepository = transactionRepository;
        _providerBankRepository = providerBankRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(BankTransferRequestDTO dto, CancellationToken cancellationToken)
    {
        // Source bank hesabını al
        var sourceBank = await _bankAccountRepository.GetByIdAsync(dto.SourceBankId);
        if (sourceBank == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Gönderici banka hesabı bulunamadı.");

        // Yetersiz bakiye kontrolü
        if (sourceBank.Balance < dto.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Yetersiz bakiye");

        // Target bank hesabını al
        var targetBank = await _bankAccountRepository.GetByIdAsync(dto.TargetBankId);
        if (targetBank == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Alıcı banka hesabı bulunamadı.");

        // BankName kontrolü ve provider bankayı al veya oluştur
        if (string.IsNullOrWhiteSpace(dto.BankName))
            return ServiceResponse<TransactionResponseDTO>.Fail("BankName alanı zorunludur.");

        var providerBank = await _providerBankRepository.GetAsync(p => p.BankName == dto.BankName);
        if (providerBank == null)
        {
            providerBank = new ProviderBank { BankName = dto.BankName };
            await _providerBankRepository.AddAsync(providerBank);
            await _providerBankRepository.SaveChangesAsync();
        }

        // Bakiye güncelle
        sourceBank.Balance -= dto.Amount;
        targetBank.Balance += dto.Amount;

        await _bankAccountRepository.UpdateAsync(sourceBank);
        await _bankAccountRepository.UpdateAsync(targetBank);

        // Transaction oluştur
        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Type = TransactionType.BankTransfer,
            Currency = 0,
            Description = dto.Description ?? $"Banka transferi - {dto.Iban}"
        };
        await _transactionRepository.AddAsync(transaction);

        // BankTransaction oluştur
        var bankTransaction = new BankTransaction
        {
            TransactionId = transaction.Id,
            ProviderBankId = providerBank.Id,
            Iban = dto.Iban,
            TargetBankId = dto.TargetBankId,
            SourceBankId = sourceBank.ProviderBankId, // ✅
            Commission = "0"
        };
        await _bankTransactionRepository.AddAsync(bankTransaction);

        // Dönüş DTO
        var responseDto = new TransactionResponseDTO
        { 
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description
        };

        return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, "Banka transferi başarılı.");
    }
}
