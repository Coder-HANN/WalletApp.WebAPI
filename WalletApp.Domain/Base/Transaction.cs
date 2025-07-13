using WalletApp.Domain.Enums;

namespace WalletApp.Domain.Base
{
    public class Transaction : ProductClass
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public decimal Currency { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
        public Wallet Wallet { get; set; }
        public ICollection<WalletTransfer> WalletTransfers { get; set; }
        public Payment Payment { get; set; }
        public BankTransaction BankTransaction { get; set; }
    }
}
