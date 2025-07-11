using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class ProviderBank : ProductClass
    {
        public int Id { get; set; }
        public string BankName { get; set; }

        public ICollection<BankTransaction> BankTransactions { get; set; }
    }
}
