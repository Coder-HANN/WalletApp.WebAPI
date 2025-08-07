using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;
using Microsoft.EntityFrameworkCore;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Persistence.Context;

namespace WalletApp.Persistence.Repositories
{
    public class BankAccountRepository : EfEntityRepositoryBase<AppBankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(WalletDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<AppBankAccount>> GetUserAccountsAsync(int userId)
        {
            return await _context.BankAccounts
                .Where(x => x.AppUserId == userId && !x.IsDelete)
                .ToListAsync();
        }

        Task IBankAccountRepository.AddAsync(AppBankAccount entity)
        {
            return AddAsync(entity);
        }
        public async Task DeleteAsync(AppBankAccount bankAccount)
        {
            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync();
        }
    }
}
