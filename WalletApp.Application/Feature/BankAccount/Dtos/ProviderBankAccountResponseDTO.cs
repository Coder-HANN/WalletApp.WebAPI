using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.BankAccount.Dtos
{
    public class ProviderBankAccountResponseDTO
    {
        public string BankName { get; set; } = null!;
        public string Iban { get; set; } = null!;
        public string AccountName { get; set; } = null!;

    }
}
