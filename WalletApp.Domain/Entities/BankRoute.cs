
namespace WalletApp.Domain.Entities
{
    public class BankRoute : BaseEntity
    {
        public string TargetBankCode { get; set; }
        public string ProviderBankCode { get; set; }
    }
}
