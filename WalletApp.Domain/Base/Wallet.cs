using System.Text.Json.Serialization;

namespace WalletApp.Domain.Base
{
    public class Wallet : ProductClass
    {   
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalBalance { get; set; }
        public string Assest { get; set; }
        public string Currency { get; set; } = "TL";
        
        public User User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<WalletTransfer> WalletTransfers { get; set; }   
    }
}
