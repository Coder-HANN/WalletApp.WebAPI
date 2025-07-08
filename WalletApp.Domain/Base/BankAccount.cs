



namespace WalletApp.Domain.Base
{
    public class BankAccount : ProductClass
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WalletId { get; set; }
        public string Bilgiler { get; set; }
        public User User { get; set; }
        
    }
}
