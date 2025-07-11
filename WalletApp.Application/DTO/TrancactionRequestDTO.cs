using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.DTO
{
    public class TransactionRequestDTO
    {
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransectionType Type { get; set; }  //  enum olarak aldık
        public string? Description { get; set; }
    }
}
