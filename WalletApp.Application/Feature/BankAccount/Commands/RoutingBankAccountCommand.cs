
using MediatR;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class RoutingBankAccountCommand:IRequest<ServiceResponse<RoutingBankAccountResponseDTO>>
    {
        public Guid SourceProviderBankId { get; set; }
        public Guid? TargetProviderBankId { get; set; }
    }
}
