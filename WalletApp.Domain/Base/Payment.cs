using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class Payment : ProductClass
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string Institution { get; set; }
        public Transaction Transaction { get; set; }


    }
}
