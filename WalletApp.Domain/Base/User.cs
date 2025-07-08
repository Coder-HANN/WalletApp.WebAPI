using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserDetail UserDetail { get; set; }
        public ICollection<Wallet> Wallet { get; set; }
        public ICollection<BankAccount> BankaHesap { get; set; }
    }

};
