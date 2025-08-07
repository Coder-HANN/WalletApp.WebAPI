using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class DeleteBankAccountCommand : IRequest<ServiceResponse<string>>
    {
        public Guid WalletId { get; set; }
        public string Iban { get; set; }
    }
}
