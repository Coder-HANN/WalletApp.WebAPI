using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class Wallet : ProductClass
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalBalance { get; set; }
       
        public string Assest { get; set; }
        public User User { get; set; }
        public ICollection<Transection> Transections { get; set; }
        public ICollection<WalletTransfer> WalletTransfers { get; set; }
        
    }

}
