using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Dtos
{
    public class ProviderBankAccountRequestDTO : IRequest<ServiceResponse<ProviderBankAccountResponseDTO>>
    {
        public string BankName { get; set; } = null!;
        public string Iban { get; set; } = null!;
        public string AccountType { get; set; } = null!;


    }
}
