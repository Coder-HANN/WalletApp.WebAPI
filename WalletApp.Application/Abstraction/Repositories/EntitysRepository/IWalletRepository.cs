
using WalletApp.Domain.Base;



namespace WalletApp.Application.Abstraction.Repositories.EntitysRepository
{
    public interface IWalletRepository : IEntityRepository<Wallet>
    {
        Task<IEnumerable<Wallet>> GetAllByUserIdAsync(int userId);
    }
}
