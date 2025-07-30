

namespace WalletApp.Domain.Entities
{
    public class AppPayment : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public string Institution { get; set; }
        public Transaction Transaction { get; set; }
        public decimal Amount { get; set; }
    }
}
