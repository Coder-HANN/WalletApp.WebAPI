using WalletApp.Domain.Base;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Persistence.Base;

namespace WalletApp.Persistence.Repositories
{
    public class BankAccountRepository : EfEntityRepositoryBase<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(WalletDbContext context) : base(context)
        {
        }
    }
}
