using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.DTO
{
    public class TransferRequestDTO
    {
        public int SourceWalletId { get; set; }
        public int TargetWalletId { get; set; }
        public decimal Amount { get; set; }

    }
}