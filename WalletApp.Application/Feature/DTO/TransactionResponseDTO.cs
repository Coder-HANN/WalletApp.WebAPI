using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.DTO
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
