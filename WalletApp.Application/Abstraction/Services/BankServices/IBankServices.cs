using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Infrastructure.Services.BankServices
{
    public interface IBankServices
    {
        Task<decimal> BakiyeBilgisi(BankTransferCommand command);
        Task<Unit> ParaTransferi(BankTransferCommand command);
    }
}
