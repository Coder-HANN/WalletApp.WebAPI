
namespace WalletApp.Domain.Base
{
    public class BankAccount : BaseEntity
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid WalletId { get; set; }
        public string Bilgiler { get; set; }
        public User User { get; set; }
        
    }
}
