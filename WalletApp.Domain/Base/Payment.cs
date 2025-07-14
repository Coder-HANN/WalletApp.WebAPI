

namespace WalletApp.Domain.Base
{
    public class Payment : ProductClass
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public string Institution { get; set; }
        public Transaction Transaction { get; set; }
    }
}
