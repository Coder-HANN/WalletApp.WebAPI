

namespace WalletApp.Application.DTO
{
    public class BankTransferRequestDTO
    {
        public Guid WalletId { get; set; }
        public Guid ProviderBankId { get; set; }
        public Guid TargetBankId { get; set; }
        public Guid SourceBankId { get; set; }
        public int Iban { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

}
