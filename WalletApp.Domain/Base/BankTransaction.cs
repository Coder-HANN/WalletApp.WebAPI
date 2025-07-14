using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WalletApp.Domain.Base
{
    public class BankTransaction : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid ProviderBankId { get; set; }
        public int Iban { get; set; }
        public Guid TargetBankId { get; set; } 
        public Guid SourceBankId { get; set; }
        public string Commission { get; set; }
        public Transaction Transaction { get; set; }

        public ProviderBank ProviderBank { get; set; } 
    }
}
