using WalletApp.Domain.Entities;

namespace WalletApp.Application.Abstraction.Repositories
{
    public interface IBankAccountRepository : IEntityRepository<AppBankAccount>
    {
        Task AddAsync(AppBankAccount entity);
        Task<IEnumerable<AppBankAccount>> GetUserAccountsAsync(int userId);
        Task <int>SaveChangesAsync();
        

    }
}
