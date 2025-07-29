using WalletApp.Domain.Entities;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Persistence.Base;

namespace WalletApp.Persistence.Repositories
{
    public class BankTransactionRepository : EfEntityRepositoryBase<BankTransaction>, IBankTransactionRepository
    {
        public BankTransactionRepository(WalletDbContext context) : base(context) { }
    }
}
