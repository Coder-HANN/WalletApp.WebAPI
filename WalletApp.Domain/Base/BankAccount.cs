
namespace WalletApp.Domain.Base
{
    public class BankAccount : BaseEntity
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
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

        public User User { get; set; }
    }
}
