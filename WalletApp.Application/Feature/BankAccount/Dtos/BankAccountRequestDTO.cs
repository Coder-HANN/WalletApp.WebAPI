using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class BankAccountRequestDTO : IRequest<ServiceResponse<BankAccountRequestDTO>>
    
    { 
        public string AccountName { get; set; }
        public string Iban { get; set; }
        public string BankName { get; set; }
        public string AccountType { get; set; } // Örnek: "Vadesiz", "Vadeli" Vadeli vadesiz cüzdan eklenecek 
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } 
        public int AppUserId { get; set; }
        public Guid WalletId { get; set; }
    }
    
}
