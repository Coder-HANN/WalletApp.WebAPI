using MediatR;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Application.DTO;

namespace WalletApp.Application.Handlers.Bank
{
    public class BankTransferCommandHandler : IRequestHandler<BankTransferCommand, Transection>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransectionRepository _transectionRepository;
        private readonly IBankTransectionRepository _bankTransectionRepository;
        private readonly IProviderBankRepository _providerBankRepository;

        public BankTransferCommandHandler(
            IWalletRepository walletRepository,
            ITransectionRepository transectionRepository,
            IBankTransectionRepository bankTransectionRepository,
            IProviderBankRepository providerBankRepository)
        {
            _walletRepository = walletRepository;
            _transectionRepository = transectionRepository;
            _bankTransectionRepository = bankTransectionRepository;
            _providerBankRepository = providerBankRepository;
        }

        public async Task<Transection> Handle(BankTransferCommand request, CancellationToken cancellationToken)
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
            var transection = new Transection
            {
                WalletId = wallet.Id,
                Amount = dto.Amount,
                Type = 3, // 3 = Banka Transferi
                Currency = 0, // TL
                Description = dto.Description ?? $"Banka transferi - {dto.Iban}"
            };
            await _transectionRepository.AddAsync(transection);

            // 3. Banka işlemi oluştur
            var bankTransection = new BankTransection
            {
                TransactionId = transection.Id,
                ProviderBankId = providerBank.Id,
                Iban = dto.Iban,
                TargetBankId = dto.TargetBankId,
                SourceBankId = dto.SourceBankId,
                Commission = "0" // opsiyonel
            };
            await _bankTransectionRepository.AddAsync(bankTransection);

            return transection;
        }
    }
}
