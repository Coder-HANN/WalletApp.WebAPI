
using Microsoft.AspNetCore.Identity;
using WalletApp.Domain.Enums;

namespace WalletApp.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; } 
        public UserDetail UserDetail { get; set; }
        public ICollection<AppWallet> Wallet { get; set; }
        public ICollection<AppBankAccount> BankHesap { get; set; }
        
    }
};