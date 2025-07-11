using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.DTO
{
    public class BankTransferRequestDTO
    {
        public int WalletId { get; set; }
        public int ProviderBankId { get; set; }
        public int TargetBankId { get; set; }
        public int SourceBankId { get; set; }
        public int Iban { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

}
