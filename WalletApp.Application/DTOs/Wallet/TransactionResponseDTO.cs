using WalletApp.Domain.Enums;
using MediatR;

namespace WalletApp.Application.DTOs.Wallet
{
    public class TransactionResponseDTO 
    
    {
        public Guid Id { get; set; }
        public int AppUserId { get; set; }
        public Guid? WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DescriptionType Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Suggestion { get; set; }
    }
}
