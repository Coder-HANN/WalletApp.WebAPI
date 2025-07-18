using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Entities;



namespace WalletApp.Application.Services.EntitiesRepositories
{
    public interface IWalletRepository : IEntityRepository<AppWallet>
    {
        Task<AppWallet> AddAsync(AppWallet wallet);
        Task<AppWallet> DeleteAsync(AppWallet wallet);
        Task<IEnumerable<AppWallet>> GetAllByAppUserIdAsync(int AppUserId);
        Task<AppWallet> UpdateAsync(AppWallet wallet);
    }
}
