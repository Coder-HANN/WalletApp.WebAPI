using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence.Context;

namespace WalletApp.Persistence.Base
{
    
    public class TransactionRepository: EfEntityRepositoryBase<Transaction>, ITransactionRepository
    {
       
        public TransactionRepository(WalletDbContext context) : base(context){ }

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
