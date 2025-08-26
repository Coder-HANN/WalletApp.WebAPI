using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Domain.Entities;

namespace WalletApp.Infrastructure.Services.BankServices
{
    public interface IBankServicesFactory
    {
        IBankServices SelectBankServices(string bankCode);

    }
}
