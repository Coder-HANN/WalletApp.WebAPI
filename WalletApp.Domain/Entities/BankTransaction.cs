using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletApp.Domain.Entities
{
    public class BankTransaction : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }

        public Guid? ProviderBankId { get; set; }
        public ProviderBank ProviderBank { get; set; }

        public Guid? SourceBankId { get; set; }
        [ForeignKey("SourceBankId")]
        public AppBankAccount? SourceBankAccount { get; set; }

        public Guid? TargetProviderBankId { get; set; }
        public ProviderBank? TargetProviderBank { get; set; }

        public Guid? TargetAppBankAccountId { get; set; }
        public AppBankAccount? TargetAppBankAccount { get; set; }

        public string Iban { get; set; }
        public string Commission { get; set; }

        public Transaction Transaction { get; set; }
    }
}
