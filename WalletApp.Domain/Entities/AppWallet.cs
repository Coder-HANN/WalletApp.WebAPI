using System.Text.Json.Serialization;

namespace WalletApp.Domain.Entities
{
    public class AppWallet : BaseEntity
    {   
        public Guid Id { get; set; }
        public int AppUserId { get; set; }
        public decimal TotalBalance { get; set; }
        public string Assest { get; set; }
        public string Currency { get; set; } = "TL";
        
        public AppUser User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<WalletTransfer> WalletTransfers { get; set; }   
    }
}
