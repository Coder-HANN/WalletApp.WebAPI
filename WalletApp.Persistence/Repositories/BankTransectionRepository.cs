using System.Linq.Expressions;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;



namespace WalletApp.Persistence.Base
{
    public class BankTransactionRepository : EfEntityRepositoryBase<BankTransaction>, IBankTransactionRepository
    {

        public BankTransactionRepository(WalletDbContext context) : base(context) { }

        public BankTransaction Add(BankTransaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<BankTransaction> AddAsync(BankTransaction entity)
        {
            throw new NotImplementedException();
        }

        public BankTransaction Delete(BankTransaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<BankTransaction> DeleteAsync(BankTransaction entity)
        {
            throw new NotImplementedException();
        }

        public BankTransaction Get(Expression<Func<BankTransaction, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankTransaction> GetAll(Expression<Func<BankTransaction, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BankTransaction>> GetAllAsync(Expression<Func<BankTransaction, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<BankTransaction> GetAsync(Expression<Func<BankTransaction, bool>> predicate, Func<IQueryable<BankTransaction>, IQueryable<BankTransaction>> include = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BankTransaction> Query()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public BankTransaction Update(BankTransaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<BankTransaction> UpdateAsync(BankTransaction entity)
        {
            throw new NotImplementedException();
        }
    }
    public class ProviderBankRepository : EfEntityRepositoryBase<ProviderBank>, IProviderBankRepository
    {
        public ProviderBankRepository(WalletDbContext context) : base(context) { }

        public ProviderBank Add(ProviderBank entity)
        {
            throw new NotImplementedException();
        } 

        public Task<ProviderBank> AddAsync(ProviderBank entity)
        {
            throw new NotImplementedException();
        }

        public ProviderBank Delete(ProviderBank entity)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderBank> DeleteAsync(ProviderBank entity)
        {
            throw new NotImplementedException();
        }

        public ProviderBank Get(Expression<Func<ProviderBank, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProviderBank> GetAll(Expression<Func<ProviderBank, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProviderBank>> GetAllAsync(Expression<Func<ProviderBank, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderBank> GetAsync(Expression<Func<ProviderBank, bool>> predicate, Func<IQueryable<ProviderBank>, IQueryable<ProviderBank>> include = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProviderBank> Query()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public ProviderBank Update(ProviderBank entity)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderBank> UpdateAsync(ProviderBank entity)
        {
            throw new NotImplementedException();
        }
    }

}
