using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class BankAccountCommand : IRequest<ServiceResponse<BankAccountCommand>>
    
    { 
        public string AccountName { get; set; }
        public string Iban { get; set; }
        public string BankName { get; set; }
        public AccountType AccountType { get; set; } 
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } 
        public Guid WalletId { get; set; }

        public BankAccountCommand() { }

    }
    
}
