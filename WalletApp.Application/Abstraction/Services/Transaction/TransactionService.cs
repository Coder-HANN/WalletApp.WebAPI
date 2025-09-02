using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WalletApp.Application.Abstraction.Services.Transaction
{

    public class TransactionService : ITransactionService
    {
        private readonly DbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public TransactionService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Begin()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
        }
    }
}
