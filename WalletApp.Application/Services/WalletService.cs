
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Application.DTO;
using WalletApp.Domain.Base;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Services
{
    public class WalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletTransferRepository _walletTransferRepository;

        public WalletService(
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            IWalletTransferRepository walletTransferRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _walletTransferRepository = walletTransferRepository;
        }

        public async Task<Wallet> CreateWalletAsync(int userId, string assest)
        {
            var wallet = new Wallet
            {
                UserId = userId,
                Assest = assest,
                TotalBalance = 0
            };

            return await _walletRepository.AddAsync(wallet);
        }

        public async Task<IEnumerable<Wallet>> GetWalletsByUserIdAsync(int userId)
        {
            return await _walletRepository.GetAllAsync(w => w.UserId == userId);
        }

        public async Task<Wallet> GetWalletByIdAsync(Guid walletId)
        {
            return await _walletRepository.GetAsync(w => w.Id == walletId);
        }

        public async Task<Wallet> UpdateWalletAsync(Wallet wallet)
        {
            return await _walletRepository.UpdateAsync(wallet);
        }

        public async Task<Wallet> DeleteWalletAsync(Wallet wallet)
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

            // Entity yerine DTO dön
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
