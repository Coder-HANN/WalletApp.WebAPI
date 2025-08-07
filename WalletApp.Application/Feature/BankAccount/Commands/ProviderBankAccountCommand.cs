using MediatR;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class ProviderBankAccountCommand : IRequest<ServiceResponse<ProviderBankAccountResponseDTO>>
    {
        public string BankName { get; set; } = null!;
        public string Iban { get; set; } = null!;
        public AccountType AccountType { get; set; }

    }
}
