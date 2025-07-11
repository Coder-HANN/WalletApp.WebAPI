using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Domain.Base;
using static WalletApp.Application.Abstraction.Repositories.EntitysRepository.IBankTransactionRepository;

namespace WalletApp.Persistence.Base
{
    public class BankTransectionRepository : EfEntityRepositoryBase<BankTransection, WalletDbContext>, IBankTransectionRepository
    {
        public BankTransectionRepository(WalletDbContext context) : base(context) { }
    }
    public class ProviderBankRepository : EfEntityRepositoryBase<ProviderBank, WalletDbContext>, IProviderBankRepository
    {
        public ProviderBankRepository(WalletDbContext context) : base(context) { }
    }

}
