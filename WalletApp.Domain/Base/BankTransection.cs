using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class BankTransection : ProductClass
    {
        public int Id { get; set; }
        public int TransectionId { get; set; }
        public int ProviderBankId { get; set; }
        public int Iban { get; set; }
        public int TargetBankId { get; set; } 
        public int SourceBankId { get; set; }
        public string Commission { get; set; }
        public Transection Transection { get; set; }

        public ProviderBank ProviderBank { get; set; } 
    }
}
