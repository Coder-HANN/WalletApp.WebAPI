using WalletApp.Domain.Base;
using WalletApp.Persistence.Base;
using WalletApp.Application.Services.Repositories.EntitysRepository;

namespace WalletApp.Persistence.Repositories
{
    public class BankAccountRepository : EfEntityRepositoryBase<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(WalletDbContext context) : base(context)
        {
        }
    }
}
