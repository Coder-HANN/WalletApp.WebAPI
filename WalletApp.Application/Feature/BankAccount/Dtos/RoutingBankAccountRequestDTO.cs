
using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Dtos
{
    public class RoutingBankAccountRequestDTO:IRequest<ServiceResponse<RoutingBankAccountResponseDTO>>
    {
        public string Iban { get; set; }
    }
}
