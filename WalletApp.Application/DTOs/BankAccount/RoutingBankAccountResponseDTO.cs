using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.DTOs.BankAccount
{
    public class RoutingBankAccountResponseDTO
    {
        public Guid SourceProviderBankId { get; set; }
        public string SourceProviderBankName { get; set; } = null!;

        public Guid? TargetBankAccountId { get; set; }
        public string? TargetBankName { get; set; }
    }
}
