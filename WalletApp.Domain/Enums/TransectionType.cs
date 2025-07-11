using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Enums
{
    public enum TransectionType
    {
        Deposit = 0,
        Withdraw = 1,
        Transfer = 2,
        BankTransfer = 3,
        Payment = 4
    }
}
