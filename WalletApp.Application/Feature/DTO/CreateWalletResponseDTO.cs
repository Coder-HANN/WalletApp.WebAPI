namespace WalletApp.Application.Feature.DTO
{
    public class CreateWalletResponseDTO
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalBalance { get; set; }
        public string Assest { get; set; }
        public object Currency { get; internal set; }
    }
}
