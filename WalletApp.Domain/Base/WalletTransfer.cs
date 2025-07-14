namespace WalletApp.Domain.Base
{
    public class WalletTransfer : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid WalletId { get; set; }
        public Guid SourceWalletId { get; set; } // source wallet id

        public string Target { get; set; } // string mi
        public int IslemNo { get; set; }
        public Wallet Wallet { get; set; }
        public Transaction Transaction { get; set; }
    }
}


