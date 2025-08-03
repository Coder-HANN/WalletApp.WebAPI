using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;

namespace WalletApp.Persistence.Repositories
{
    public class UserRepository : EfEntityRepositoryBase<AppUser>, IUserRepository
    {
        public UserRepository(WalletDbContext context) : base(context)
        {
        }
        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
        }
        public async Task<AppUser> GetAsync(Expression<Func<AppUser, bool>> predicate, Func<IQueryable<AppUser>, IQueryable<AppUser>> include = null)
        {
            IQueryable<AppUser> query = _context.Users;

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync();
        }

        Task IUserRepository.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
        public async Task DeleteAsync(AppUser user)
        {
            // BankAccounts sil
            var bankAccounts = _context.BankAccounts.Where(b => b.AppUserId == user.Id);
            _context.BankAccounts.RemoveRange(bankAccounts);

            // Walletları liste olarak çek
            var wallets = _context.Wallets.Where(w => w.AppUserId == user.Id).ToList();

            foreach (var wallet in wallets)
            {
                // Wallet'a bağlı Transactions
                var transactions = _context.Transactions.Where(t => t.WalletId == wallet.Id).ToList();

                foreach (var transaction in transactions)
                {
                    // Transactions'a bağlı WalletTransfers
                    var walletTransfers = _context.WalletTransfers.Where(wt => wt.TransactionId == transaction.Id);
                    _context.WalletTransfers.RemoveRange(walletTransfers);

                    // Transactions'a bağlı Payments
                    var payments = _context.Payments.Where(p => p.TransactionId == transaction.Id);
                    _context.Payments.RemoveRange(payments);
                }

                // Transactions sil
                _context.Transactions.RemoveRange(transactions);
            }

            // Walletlar sil
            _context.Wallets.RemoveRange(wallets);

            // Kullanıcıyı sil
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        public async Task<AppUser> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
    
}

