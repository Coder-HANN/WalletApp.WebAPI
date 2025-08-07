using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Persistence.Context;

namespace WalletApp.Persistence.Repositories
{
    public class BankTransactionRepository : EfEntityRepositoryBase<BankTransaction>, IBankTransactionRepository
    {
        public BankTransactionRepository(WalletDbContext context) : base(context) { }
    }
}
