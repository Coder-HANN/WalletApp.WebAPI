using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.UnitOfWork;
using WalletApp.Persistence.Context;
namespace WalletApp.Application.Abstraction.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WalletDbContext context;
        public UnitOfWork(WalletDbContext context,
                      IBankAccountRepository bankAccountRepository,
                      IBankTransactionRepository bankTransactionRepository,
                      ITransactionRepository transactionRepository)
        {
            this.context = context;
            BankAccountRepository = bankAccountRepository;
            BankTransactionRepository = bankTransactionRepository;
            TransactionRepository = transactionRepository;
        }


        public IBankAccountRepository BankAccountRepository { get; set ; }
        public IBankTransactionRepository BankTransactionRepository { get; set; }
        public ITransactionRepository TransactionRepository { get; set; }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}

