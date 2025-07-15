using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Base;



namespace WalletApp.Application.Services.Repositories.EntitysRepository
{
    public interface IWalletRepository : IEntityRepository<Wallet>
    {
        Task<IEnumerable<Wallet>> GetAllByUserIdAsync(int userId);
    }
}
