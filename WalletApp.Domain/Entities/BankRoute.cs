
namespace WalletApp.Domain.Entities
{
    public class BankRoute : BaseEntity
    {
        /// <summary>
        /// TargetBankProviderId
        /// </summary>
        /// 
        
        public Guid? TargetBankId { get; set; }
        /// <summary>
        /// SourceBankProviderId
        /// </summary>
        public Guid? SourceBankId { get; set; }

        public string? Remark { get; set; }
    }
}
