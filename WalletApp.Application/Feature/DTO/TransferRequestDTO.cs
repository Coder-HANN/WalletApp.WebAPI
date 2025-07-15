namespace WalletApp.Application.Feature.DTO
{
    public class TransferRequestDTO
    {
        public Guid SourceWalletId { get; set; }
        public Guid TargetWalletId { get; set; }
        public decimal Amount { get; set; }
    }
}