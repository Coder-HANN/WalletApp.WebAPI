using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
        
namespace WalletApp.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserDetail UserDetail { get; set; }
        public ICollection<AppWallet> Wallet { get; set; }
        public ICollection<AppBankAccount> BankaHesap { get; set; }
        [NotMapped]
        public ClaimsIdentity? Name { get; set; }
    }
};