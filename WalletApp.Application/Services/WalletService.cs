using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Domain.Base;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Services
{
    public class WalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletTransferRepository _walletTransferRepository;

        // Tüm repository'leri tek constructor'da inject et
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
            var wallets = await _walletRepository.GetAllAsync(w => w.UserId == userId);

            var result = wallets.Select(w => new Wallet
            {
                Id = w.Id,
                UserId = w.UserId,
                Assest = w.Assest,
                TotalBalance = w.TotalBalance,
                Currency = w.Currency
            }).ToList();

            return result;
        }

        public async Task<Wallet> GetWalletByIdAsync(int walletId)
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

        public async Task<Transaction> ProcessWalletTransactionAsync(int walletId, decimal amount, TransectionType type, string? description)

        {
            var wallet = await _walletRepository.GetAsync(w => w.Id == walletId);
            if (wallet == null)
                throw new Exception("Cüzdan bulunamadı");

            if (type == "Withdraw" && wallet.TotalBalance < amount)
                throw new Exception("Yetersiz bakiye");

            if (type == TransectionType.Deposit)
                wallet.TotalBalance += amount;
            else if (type == "Withdraw")
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
            return transaction;
        }

        public async Task<Transaction> TransferAsync(int sourceWalletId, int targetWalletId, decimal amount)
        {
            var source = await _walletRepository.GetAsync(w => w.Id == sourceWalletId);
            var target = await _walletRepository.GetAsync(w => w.Id == targetWalletId);

            if (source == null || target == null)
                throw new Exception("Cüzdan(lar) bulunamadı");

            if (source.TotalBalance < amount)
                throw new Exception("Yetersiz bakiye");

            // 1. Kaynaktan düş
            source.TotalBalance -= amount;
            await _walletRepository.UpdateAsync(source);

            // 2. Hedefe ekle
            target.TotalBalance += amount;
            await _walletRepository.UpdateAsync(target);

            // 3. Transaction kaydı oluştur
            var transaction = new Transaction
            {
                WalletId = sourceWalletId,
                Amount = amount,
                Type = "Transfer",
                Description = $"Cüzdanlar arası transfer: {targetWalletId}"
            };
            await _transactionRepository.AddAsync(transaction);

            // 4. WalletTransfer kaydı
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

        public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(int walletId)
        {
            return await _transactionRepository.GetAllAsync(t => t.WalletId == walletId);
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
