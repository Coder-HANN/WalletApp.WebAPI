using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class User : ProductClass
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        
        public UserDetail UserDetail { get; set; }
        public ICollection<Wallet> Wallet { get; set; }
        public ICollection<BankAccount> BankaHesap { get; set; }

        [NotMapped]
        public ClaimsIdentity? Name { get; set; }
    }
};
