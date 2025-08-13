using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class DeleteBankAccountCommand : IRequest<ServiceResponse<string>>
    {
        public string Iban { get; set; }
    }
}
