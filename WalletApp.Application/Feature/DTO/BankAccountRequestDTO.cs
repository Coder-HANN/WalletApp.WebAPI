using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.DTO
{
    public class BankAccountRequestDTO { 
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public string Iban { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountType { get; set; } // Örnek: "Vadesiz", "Vadeli"
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    
}
