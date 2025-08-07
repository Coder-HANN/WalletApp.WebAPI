using WalletApp.Domain.Entities;



namespace WalletApp.Application.Abstraction.Repositories
{
    public interface IWalletRepository : IEntityRepository<AppWallet>
    {
        Task<AppWallet> AddAsync(AppWallet wallet);
        Task<AppWallet> DeleteAsync(AppWallet wallet);
        Task<IEnumerable<AppWallet>> GetAllByAppUserIdAsync(int AppUserId);
        Task<AppWallet> GetByUserIdAsync(int currentUserId);
        Task <int>SaveChangesAsync();
        Task<AppWallet> UpdateAsync(AppWallet wallet);
    }
}
