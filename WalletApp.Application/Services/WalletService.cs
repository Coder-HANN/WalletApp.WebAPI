using System.Collections.Generic;
using System.Threading.Tasks;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Domain.Base;

namespace WalletApp.Application.Services
{
    public class WalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        /// <summary>
        /// Yeni bir cüzdan oluşturur.
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="assest">Cüzdanın varlık türü (örnek: TL, USD, Kripto)</param>
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

        /// <summary>
        /// Belirli bir kullanıcıya ait tüm cüzdanları getirir.
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        public async Task<IEnumerable<Wallet>> GetWalletsByUserIdAsync(int userId)
        {
            return await _walletRepository.GetAllAsync(w => w.UserId == userId);
        }

        /// <summary>
        /// Belirli bir cüzdanı getirir.
        /// </summary>
        public async Task<Wallet> GetWalletByIdAsync(int walletId)
        {
            return await _walletRepository.GetAsync(w => w.Id == walletId);
        }

        /// <summary>
        /// Cüzdan günceller (örneğin bakiye).
        /// </summary>
        public async Task<Wallet> UpdateWalletAsync(Wallet wallet)
        {
            return await _walletRepository.UpdateAsync(wallet);
        }

        /// <summary>
        /// Cüzdanı siler.
        /// </summary>
        public async Task<Wallet> DeleteWalletAsync(Wallet wallet)
        {
            return await _walletRepository.DeleteAsync(wallet);
        }
    }
}
