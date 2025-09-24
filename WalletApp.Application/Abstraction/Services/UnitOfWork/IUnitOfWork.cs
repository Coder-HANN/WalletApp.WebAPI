using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Application.Abstraction.Repositories;

namespace WalletApp.Application.Abstraction.Services.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBankAccountRepository BankAccountRepository { get; set; }
        IBankTransactionRepository BankTransactionRepository { get; set; }
        ITransactionRepository TransactionRepository { get; set; }

        Task<int> SaveChangeAsync();
    }
}
