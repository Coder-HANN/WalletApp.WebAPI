
using MediatR;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class RoutingBankAccountCommand:IRequest<ServiceResponse<RoutingBankAccountResponseDTO>>
    {
        public string Iban { get; set; }
    }
}
