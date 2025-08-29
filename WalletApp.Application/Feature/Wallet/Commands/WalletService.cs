using Microsoft.AspNetCore.Http;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Commands;
using WalletApp.Application.Feature.Wallet.Validations.Resource;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Infrastructure.Services.MemoryCach;


namespace WalletApp.Application.Feature.Wallet.Handlers
{
    public class WalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletTransferRepository _walletTransferRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProviderBankRepository _providerBankRepository;
        private readonly IBankTransactionRepository _bankTransactionRepository;
        private readonly ICurrentUserService _currentUserService;

        public WalletService(
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            IWalletTransferRepository walletTransferRepository,
            IHttpContextAccessor httpContextAccessor,
            IProviderBankRepository providerBankRepository,
            IBankTransactionRepository bankTransactionRepository,
            ICurrentUserService currentUserService)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _walletTransferRepository = walletTransferRepository;
            _httpContextAccessor = httpContextAccessor;
            _providerBankRepository = providerBankRepository;
            _bankTransactionRepository = bankTransactionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<AppWallet> CreateWalletAsync(string assest, CancellationToken ct)
        {
            var currentUserId = _currentUserService.CurrentUser();
            if (currentUserId == null || currentUserId == -1)
                throw new Exception(WalletResource.UserIsNotFound);

            var wallet = new AppWallet
            {
                AppUserId = currentUserId,
                Assest = assest,
                TotalBalance = 0
            };

            return await _walletRepository.AddAsync(wallet);
        }

        public async Task<IEnumerable<AppWallet>> GetMyWalletsAsync(int currentUserId)
        {

            if (currentUserId == null || currentUserId == -1)
                throw new Exception(WalletResource.UserIsNotFound);

            return await _walletRepository.GetAllAsync(w => w.AppUserId == currentUserId);
        }



        public async Task<AppWallet> UpdateWalletAsync(AppWallet wallet)
        {
            return await _walletRepository.UpdateAsync(wallet);
        }

        public async Task<AppWallet> DeleteWalletAsync(AppWallet wallet)
        {
            return await _walletRepository.DeleteAsync(wallet);
        }

        public async Task<List<TransactionResponseDTO>> ProcessWalletTransactionAsync(Guid walletId, TransferCommand request)
        {
            var wallet = await _walletRepository.GetAsync(w => w.Id == request.SourceWalletId)
                         ?? throw new Exception(WalletResource.WalletIsNotFound);

            if (request.Type == TransactionType.Withdraw && wallet.TotalBalance < request.Amount)
                throw new Exception(WalletResource.InsufficientAmount);

            if (request.Type == TransactionType.Deposit)
                wallet.TotalBalance += request.Amount;
            else if (request.Type == TransactionType.Withdraw)
                wallet.TotalBalance -= request.Amount;

            await _walletRepository.UpdateAsync(wallet);

            var sourceTransaction = new Transaction
            {
                WalletId = request.SourceWalletId,
                Amount = -request.Amount,
                Type = TransactionType.Transfer,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(sourceTransaction);

            var targetTransaction = new Transaction
            {
                WalletId = request.TargetWalletId,
                Amount = request.Amount,
                Type = TransactionType.Transfer,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(targetTransaction);

            var islemNO = new Random().Next(100000, 999999);
            var transfer = new WalletTransfer
            {
                WalletId = request.SourceWalletId,
                SourceWalletId = request.SourceWalletId,
                Target = request.TargetWalletId.ToString(),
                TransactionId = sourceTransaction.Id,
                IslemNo = islemNO
            };
            await _walletTransferRepository.AddAsync(transfer);

            var result = new List<TransactionResponseDTO>
            {
                new TransactionResponseDTO
                {
                    Id = sourceTransaction.Id,
                    WalletId = sourceTransaction.WalletId,
                    Amount = sourceTransaction.Amount,
                    Type = sourceTransaction.Type,
                    Description = sourceTransaction.Description,
                    CreatedDate = sourceTransaction.CreatedDate
                },
                new TransactionResponseDTO
                {
                    Id = targetTransaction.Id,
                    WalletId = targetTransaction.WalletId,
                    Amount = targetTransaction.Amount,
                    Type = targetTransaction.Type,
                    Description = targetTransaction.Description,
                    CreatedDate = targetTransaction.CreatedDate
                }
            };
            return result;
        }

        public async Task<List<TransactionResponseDTO>> TransferAsync(Guid sourceWalletId, Guid targetWalletId, decimal amount, string? description)
        {
            var sourceWallet = await _walletRepository.GetAsync(w => w.Id == sourceWalletId)
                ?? throw new Exception(WalletResource.SourceWalletIsNotFound);

            var targetWallet = await _walletRepository.GetAsync(w => w.Id == targetWalletId)
                ?? throw new Exception(WalletResource.TargetWalletIsNotFound);

            if (sourceWallet.TotalBalance < amount)
                throw new Exception(WalletResource.InsufficientAmount);

            sourceWallet.TotalBalance -= amount;
            targetWallet.TotalBalance += amount;

            await _walletRepository.UpdateAsync(sourceWallet);
            await _walletRepository.UpdateAsync(targetWallet);

            var sourceTransaction = new Transaction
            {
                WalletId = sourceWalletId,
                Amount = -amount,
                Type = TransactionType.Transfer,
                Description = DescriptionType.BireyselOdeme,
                CreatedDate = DateTime.UtcNow
            };

            var targetTransaction = new Transaction
            {
                WalletId = targetWalletId,
                Amount = amount,
                Type = TransactionType.Transfer,
                Description = DescriptionType.BireyselOdeme,
                CreatedDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(sourceTransaction);
            await _transactionRepository.AddAsync(targetTransaction);

            var islemNo = new Random().Next(100000, 999999);

            await _walletTransferRepository.AddAsync(new WalletTransfer
            {
                WalletId = sourceWalletId,
                SourceWalletId = sourceWalletId,
                Target = targetWalletId.ToString(),
                TransactionId = sourceTransaction.Id,
                IslemNo = islemNo
            });

            await _walletTransferRepository.AddAsync(new WalletTransfer
            {
                WalletId = targetWalletId,
                SourceWalletId = sourceWalletId,
                Target = targetWalletId.ToString(),
                TransactionId = targetTransaction.Id,
                IslemNo = islemNo
            });

            return new List<TransactionResponseDTO>
            {
                new TransactionResponseDTO
                {
                    Id = sourceTransaction.Id,
                    WalletId = sourceTransaction.WalletId,
                    Amount = sourceTransaction.Amount,
                    Type = sourceTransaction.Type,
                    Description = sourceTransaction.Description,
                    CreatedDate = sourceTransaction.CreatedDate
                },
                new TransactionResponseDTO
                {
                    Id = targetTransaction.Id,
                    WalletId = targetTransaction.WalletId,
                    Amount = targetTransaction.Amount,
                    Type = targetTransaction.Type,
                    Description = targetTransaction.Description,
                    CreatedDate = targetTransaction.CreatedDate
                }
            };
        }

        public async Task<IEnumerable<Transaction>> GetWalletTransactionHistoryAsync(Guid walletId)
        {
            var currentUserId = _currentUserService.CurrentUser();
            if (currentUserId == null || currentUserId == -1)
                throw new Exception(WalletResource.UserIsNotFound);

            var wallet = await _walletRepository.GetAsync(w => w.Id == walletId && w.AppUserId == currentUserId);
            if (wallet == null)
                throw new Exception(WalletResource.WalletIsNotFound);

            return await _transactionRepository.GetAllAsync(t => t.WalletId == walletId);
        }
    }
}