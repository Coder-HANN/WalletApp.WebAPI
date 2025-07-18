namespace WalletApp.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class WalletTransfer : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid WalletId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid SourceWalletId { get; set; } // source wallet id

        /// <summary>
        /// 
        /// </summary>
        public string Target { get; set; } // string mi

        /// <summary>
        /// 
        /// </summary>
        public int IslemNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AppWallet Wallet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Transaction Transaction { get; set; }
    }
}


