

namespace WalletApp.Application.DTO
{
    public class CreateWalletDTO
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalBalance { get; set; }
        public string Assest { get; set; }
        public object Currency { get; internal set; }
    }
}
