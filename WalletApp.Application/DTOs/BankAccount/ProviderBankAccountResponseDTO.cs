
using WalletApp.Domain.Enums;

namespace WalletApp.Application.DTOs.BankAccount
{
    public class ProviderBankAccountResponseDTO
    {
        public string BankName { get; set; } = null!;
        public string Iban { get; set; } = null!;
        public AccountType AccountType { get; set; } 

    }
}
