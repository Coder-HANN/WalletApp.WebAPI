using MediatR;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class DepositToProviderBankAccountCommand : IRequest<ServiceResponse<DepositToProviderBankAccountResponseDTO>>
    {
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }  

        // TODO: Description enumı var oradan çekilecek 
    }
}
