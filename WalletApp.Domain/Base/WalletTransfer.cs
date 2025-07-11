using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class WalletTransfer : ProductClass
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int WalletId { get; set; }
        public int SourceWalletId { get; set; } // source wallet id

        public string Target { get; set; } // string mi
        public int IslemNo { get; set; }
        public Wallet Wallet { get; set; }
        public Transaction Transaction { get; set; }
    }
}


