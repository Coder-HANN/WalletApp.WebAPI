using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class Transection
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public decimal Currency { get; set; }
        public decimal Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public Wallet Wallet { get; set; }
        public ICollection<WalletTransfer> WalletTransfers { get; set; }
        public Payment Payment { get; set; }
        public BankTransection BankTransection { get; set; }
    }
}
