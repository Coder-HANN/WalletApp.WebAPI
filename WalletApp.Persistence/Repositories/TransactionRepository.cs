using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;

namespace WalletApp.Persistence.Base
{
    
    public class TransactionRepository: EfEntityRepositoryBase<Transaction>, ITransactionRepository
    {
       
        public TransactionRepository(WalletDbContext context) : base(context)
        {
        }

        public Task SaveChangesAsync(Transaction transaction)
        {
            return SaveChangesAsync();
        }

        Task ITransactionRepository.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}
