using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class Wallet : ProductClass
    {   
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalBalance { get; set; }
        public string Assest { get; set; }
        public string Currency { get; set; } = "TL";
        public User User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<WalletTransfer> WalletTransfers { get; set; }   
    }
}
