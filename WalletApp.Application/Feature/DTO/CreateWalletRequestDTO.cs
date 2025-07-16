
namespace WalletApp.Application.Feature.DTO
{
    public class CreateWalletRequestDTO
    {
        public int UserId { get; set; }
        public decimal TotalBalance { get; set; }
        public string Asset { get; set; }  // "Assest" yanlış yazılmış, doğrusu "Asset"
        public string Currency { get; set; }
    }
}
