
using System.Linq.Expressions;
using System.Transactions;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;

namespace WalletApp.Persistence.Base
{

    public class TransactionRepository : EfEntityRepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(WalletDbContext context) : base(context)
        {
        }

        public Domain.Base.Transaction Add(Domain.Base.Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Base.Transaction> AddAsync(Domain.Base.Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Domain.Base.Transaction Delete(Domain.Base.Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Base.Transaction> DeleteAsync(Domain.Base.Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Domain.Base.Transaction Get(Expression<Func<Domain.Base.Transaction, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Domain.Base.Transaction> GetAll(Expression<Func<Domain.Base.Transaction, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Base.Transaction>> GetAllAsync(Expression<Func<Domain.Base.Transaction, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Base.Transaction> GetAsync(Expression<Func<Domain.Base.Transaction, bool>> predicate, Func<IQueryable<Domain.Base.Transaction>, IQueryable<Domain.Base.Transaction>> include = null)
        {
            throw new NotImplementedException();
        }

        public Domain.Base.Transaction Update(Domain.Base.Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Base.Transaction> UpdateAsync(Domain.Base.Transaction entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<Domain.Base.Transaction> IEntityRepository<Domain.Base.Transaction>.Query()
        {
            throw new NotImplementedException();
        }
    }
}