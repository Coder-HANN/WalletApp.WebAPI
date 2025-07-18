using WalletApp.Domain.Enums;
using MediatR;

namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public class TransactionResponseDTO 
    
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }

        
    }
}
