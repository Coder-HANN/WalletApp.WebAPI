using MediatR;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Services.Repositories.EntitysRepository;
using WalletApp.Domain.Base;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.Handler
{
    public class BankTransferCommandHandler : IRequestHandler<BankTransferCommand, ServiceResponse<TransactionResponseDTO>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _TransactionRepository;
        private readonly IBankTransactionRepository _bankTransactionRepository;
        private readonly IProviderBankRepository _providerBankRepository;

        public BankTransferCommandHandler(
            IWalletRepository walletRepository,
            ITransactionRepository TransactionRepository,
            IBankTransactionRepository bankTransactionRepository,
            IProviderBankRepository providerBankRepository)
        {
            _walletRepository = walletRepository;
            _TransactionRepository = TransactionRepository;
            _bankTransactionRepository = bankTransactionRepository;
            _providerBankRepository = providerBankRepository;
        }

        public async Task<ServiceResponse<TransactionResponseDTO>> Handle(BankTransferCommand request, CancellationToken cancellationToken)
        {
            var dto = request.BankTransferRequest;

            var wallet = await _walletRepository.GetAsync(w => w.Id == dto.WalletId);
            if (wallet == null)
                return ServiceResponse<TransactionResponseDTO>.Fail("Cüzdan bulunamadı");

            if (wallet.TotalBalance < dto.Amount)
                return ServiceResponse<TransactionResponseDTO>.Fail("Yetersiz bakiye");

            var providerBank = await _providerBankRepository.GetAsync(p => p.Id == dto.ProviderBankId);
            if (providerBank == null)
                return ServiceResponse<TransactionResponseDTO>.Fail("Sağlayıcı banka bulunamadı");

            wallet.TotalBalance -= dto.Amount;
            await _walletRepository.UpdateAsync(wallet);

            var transaction = new Transaction
            {
                WalletId = wallet.Id,
                Amount = dto.Amount,
                Type = TransactionType.BankTransfer,
                Currency = 0,
                Description = dto.Description ?? $"Banka transferi - {dto.Iban}"
            };
            await _TransactionRepository.AddAsync(transaction);

            var bankTransaction = new BankTransaction
            {
                TransactionId = transaction.Id,
                ProviderBankId = providerBank.Id,
                Iban = dto.Iban,
                TargetBankId = dto.TargetBankId,
                SourceBankId = dto.SourceBankId,
                Commission = "0"
            };
            await _bankTransactionRepository.AddAsync(bankTransaction);

            var responseDto = new TransactionResponseDTO
            {
                WalletId = transaction.WalletId,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Description = transaction.Description
            };

            return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, "Banka transferi başarılı.");
        }
    }
}
