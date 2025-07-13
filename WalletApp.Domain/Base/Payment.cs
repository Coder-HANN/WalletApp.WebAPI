

namespace WalletApp.Domain.Base
{
    public class Payment : ProductClass
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string Institution { get; set; }
        public Transaction Transaction { get; set; }


    }
}
