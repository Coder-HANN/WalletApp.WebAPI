using MediatR;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Services.Repositories.EntitysRepository;
using WalletApp.Domain.Base;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.Handler
{
    public class BankTransferCommandHandler : IRequestHandler<BankTransferCommand, Transaction>
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

        public async Task<Transaction> Handle(BankTransferCommand request, CancellationToken cancellationToken)
        {
            var dto = request.BankTransferRequest;

            var wallet = await _walletRepository.GetAsync(w => w.Id == dto.WalletId);
            if (wallet == null)
                throw new Exception("Cüzdan bulunamadı");

            if (wallet.TotalBalance < dto.Amount)
                throw new Exception("Yetersiz bakiye");

            var providerBank = await _providerBankRepository.GetAsync(p => p.Id == dto.ProviderBankId);
            if (providerBank == null)
                throw new Exception("Sağlayıcı banka bulunamadı");

            // 1. Cüzdandan düş
            wallet.TotalBalance -= dto.Amount;
            await _walletRepository.UpdateAsync(wallet);

            // 2. İşlem oluştur
            var Transaction = new Transaction
            {
                WalletId = wallet.Id,
                Amount = dto.Amount,
                Type = TransactionType.BankTransfer, // Banka Transferi
                Currency = 0, // TL
                Description = dto.Description ?? $"Banka transferi - {dto.Iban}"
            };
            await _TransactionRepository.AddAsync(Transaction);

            // 3. Banka işlemi oluştur
            var bankTransaction = new BankTransaction
            {
                TransactionId = Transaction.Id,
                ProviderBankId = providerBank.Id,
                Iban = dto.Iban,
                TargetBankId = dto.TargetBankId,
                SourceBankId = dto.SourceBankId,
                Commission = "0" // opsiyonel
            };
            await _bankTransactionRepository.AddAsync(bankTransaction);

            return Transaction;
        }
    }
}
