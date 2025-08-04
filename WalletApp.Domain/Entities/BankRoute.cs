using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Entities
{
    public class BankRoute : BaseEntity
    {
        public string TargetBankCode { get; set; }
        public string ProviderBankCode { get; set; }
    }
}
