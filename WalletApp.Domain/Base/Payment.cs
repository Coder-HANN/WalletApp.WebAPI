using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class Payment
    {
        public int Id { get; set; }
        public int TransectionId { get; set; }
        public string Institution { get; set; }
        public Transection Transection { get; set; }


    }
}
