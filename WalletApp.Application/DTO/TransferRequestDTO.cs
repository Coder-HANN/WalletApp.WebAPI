using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Domain.Base;
namespace WalletApp.Application.DTO
{
    public class TransferRequestDTO
    {
        public Guid SourceWalletId { get; set; }
        public Guid TargetWalletId { get; set; }
        public decimal Amount { get; set; }
    }
}