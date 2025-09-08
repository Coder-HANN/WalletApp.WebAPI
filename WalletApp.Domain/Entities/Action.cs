using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Entities
{
    public class Action : BaseEntity
    {
        public Guid Id { get; set; }
        public string Remark { get; set; }
        public bool IsTransfer { get; set; }
        public decimal Amount { get; set; }
        public AppWallet AppWallet { get; set; }
        public WalletTransfer WalletTransfer { get; set; }
    }
}
