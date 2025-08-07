using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.DTOs.BankAccount
{
    public class DepositToProviderBankAccountResponseDTO
    {
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
