using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Dtos
{
    public class DepositToProviderBankAccountRequestDTO : IRequest<ServiceResponse<DepositToProviderBankAccountResponseDTO>>
    {
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public object ProviderBankId { get; internal set; }
    }
}
