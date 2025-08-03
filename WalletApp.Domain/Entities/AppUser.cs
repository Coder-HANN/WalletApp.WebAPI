
using Microsoft.AspNetCore.Identity;

namespace WalletApp.Domain.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Role { get; set; } = "User";
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public bool IsDelete { get; set; }
        public string CreatedUser { get; set; }
        public UserDetail UserDetail { get; set; }
        public ICollection<AppWallet> Wallet { get; set; }
        public ICollection<AppBankAccount> BankaHesap { get; set; }
        
    }
};