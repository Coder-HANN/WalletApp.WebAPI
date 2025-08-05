using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Services.EntitiesRepositories
{
    public interface IBankAccountRepository : IEntityRepository<AppBankAccount>
    {
        Task AddAsync(AppBankAccount entity);
        Task<IEnumerable<AppBankAccount>> GetUserAccountsAsync(int userId);
        Task <int>SaveChangesAsync();
        

    }
}
