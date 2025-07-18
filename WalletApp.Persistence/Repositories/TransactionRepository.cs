using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
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
