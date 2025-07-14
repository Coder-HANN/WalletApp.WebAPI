using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Domain.Base;          
using WalletApp.Persistence.Migrations;

namespace WalletApp.Persistence.Base
{
    
    public class TransactionRepository: EfEntityRepositoryBase<Transaction>, ITransactionRepository
    {
       
        public TransactionRepository(WalletDbContext context) : base(context)
        {
        }
    }
}
