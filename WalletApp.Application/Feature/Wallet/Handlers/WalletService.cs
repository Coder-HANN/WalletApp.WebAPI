using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.Wallet.Handlers
{
    public class WalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletTransferRepository _walletTransferRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public WalletService(
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            IWalletTransferRepository walletTransferRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _walletTransferRepository = walletTransferRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AppWallet> CreateWalletAsync(int AppUserId, string assest, CancellationToken ct)
        {
            var wallet = new AppWallet
            {
                AppUserId = AppUserId,
                Assest = assest,
                TotalBalance = 0
            };

            return await _walletRepository.AddAsync(wallet);
        }

        public async Task<IEnumerable<AppWallet>> GetWalletsByAppUserIdAsync(int AppUserId)
        {
            return (IEnumerable<AppWallet>)await _walletRepository.GetAllAsync(w => w.AppUserId == AppUserId);
        }

        public async Task<AppWallet> GetWalletByIdAsync(Guid walletId)
        {
            AppWallet wallet = await _walletRepository.GetAsync(w => w.Id == walletId);
            return wallet;
        }

        public async Task<AppWallet> UpdateWalletAsync(AppWallet wallet)
        {
            return await _walletRepository.UpdateAsync(wallet);
        }

        public async Task<AppWallet> DeleteWalletAsync(AppWallet wallet)
        {
            return await _walletRepository.DeleteAsync(wallet);
        }

        public async Task<TransactionResponseDTO> ProcessWalletTransactionAsync(Guid walletId, decimal amount, TransactionType type, string? description)
        {
            var wallet = await _walletRepository.GetAsync(w => w.Id == walletId)
                         ?? throw new Exception("Cüzdan bulunamadı");

            if (type == TransactionType.Withdraw && wallet.TotalBalance < amount)
                throw new Exception("Yetersiz bakiye");

            if (type == TransactionType.Deposit)
                wallet.TotalBalance += amount;
            else if (type == TransactionType.Withdraw)
                wallet.TotalBalance -= amount;

            await _walletRepository.UpdateAsync(wallet);

            var transaction = new Transaction
            {
                WalletId = walletId,
                Amount = amount,
                Type = type,
                Description = description
            };

            await _transactionRepository.AddAsync(transaction);

            return new TransactionResponseDTO
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Description = transaction.Description,
                CreatedDate = transaction.CreatedDate
            };
        }

        public async Task<Transaction> TransferAsync(Guid sourceWalletId, Guid targetWalletId, decimal amount)
        {
            var source = await _walletRepository.GetAsync(w => w.Id == sourceWalletId)
                         ?? throw new Exception("Kaynak cüzdan bulunamadı");

            var target = await _walletRepository.GetAsync(w => w.Id == targetWalletId)
                         ?? throw new Exception("Hedef cüzdan bulunamadı");

            if (source.TotalBalance < amount)
                throw new Exception("Yetersiz bakiye");

            source.TotalBalance -= amount;
            await _walletRepository.UpdateAsync(source);

            target.TotalBalance += amount;
            await _walletRepository.UpdateAsync(target);

            var transaction = new Transaction
            {
                WalletId = sourceWalletId,
                Amount = amount,
                Type = TransactionType.Transfer,
                Description = $"Cüzdanlar arası transfer: {targetWalletId}"
            };

            await _transactionRepository.AddAsync(transaction);

            var transfer = new WalletTransfer
            {
                WalletId = sourceWalletId,
                SourceWalletId = sourceWalletId,
                Target = targetWalletId.ToString(),
                TransactionId = transaction.Id,
                IslemNo = new Random().Next(100000, 999999)
            };

            await _walletTransferRepository.AddAsync(transfer);

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(Guid walletId)
        {
            return await _transactionRepository.GetAllAsync(t => t.WalletId == walletId);
        }

        public async Task ProcessWalletTransactionAsync(Guid walletId, decimal amount, string v, string? description)
        {
            throw new NotImplementedException();
        }
    }
}
