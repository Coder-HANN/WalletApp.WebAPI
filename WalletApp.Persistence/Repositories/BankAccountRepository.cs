using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;
using WalletApp.Application.Services.EntitiesRepositories;

namespace WalletApp.Persistence.Repositories
{
    public class BankAccountRepository : EfEntityRepositoryBase<AppBankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(WalletDbContext context) : base(context)
        {
        }

        Task IBankAccountRepository.AddAsync(AppBankAccount entity)
        {
            return AddAsync(entity);
        }
    }
}
