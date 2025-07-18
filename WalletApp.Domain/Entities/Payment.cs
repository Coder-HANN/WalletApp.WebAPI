

namespace WalletApp.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public string Institution { get; set; }
        public Transaction Transaction { get; set; }
    }
}
