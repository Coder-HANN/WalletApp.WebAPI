using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public abstract class ProductClass
    {
     public DateTime CreatedDate { get; set; }
     public DateTime? ModifiedDate { get; set; }
     public string ModifiedUser { get; set; }
     public bool IsDelete { get; set; }
        public string CreatedUser { get; set; }
        public Guid Created { get; set; }
    }
}
