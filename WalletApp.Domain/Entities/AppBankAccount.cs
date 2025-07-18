
namespace WalletApp.Domain.Entities
{
    public class AppBankAccount : BaseEntity
    {
        public Guid Id { get; set; }
        public int AppUserId { get; set; }
        public Guid WalletId { get; set; }

        public string AccountName { get; set; }
        public string Iban { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountType { get; set; }
        public string Information { get; set; }
        public decimal Balance { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public AppUser User { get; set; }
    }
}
