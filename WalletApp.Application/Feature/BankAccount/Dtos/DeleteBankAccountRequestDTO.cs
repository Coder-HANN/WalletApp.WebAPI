using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Dtos
{
    public class DeleteBankAccountRequestDTO : IRequest<ServiceResponse<string>>
    {
        public string Iban { get; set; }
    }
}
