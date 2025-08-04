

namespace WalletApp.Domain.Entities
{
    public class ProviderBank : BaseEntity
    {
        
        public Guid Id { get; set; }
        public decimal TotalBalance { get; set; }
        public string BankCode { get; set; }
        public string Iban { get; set; }
        public string BankName { get; set; }
        public ICollection<BankTransaction> BankTransactions { get; set; }
        
    }
}
