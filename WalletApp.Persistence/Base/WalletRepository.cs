using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WalletApp.Application.Services.Repositories.EntitysRepository;
using WalletApp.Domain.Base;
using WalletApp.Persistence;

namespace WalletApp.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _context;
        public WalletRepository(WalletDbContext context)
        {
            _context = context;
        }
        public async Task<Wallet?> GetByIdAsync(Guid id)
        {
            return await _context.Wallets.FindAsync(id);
        }
        public Wallet Add(Wallet entity)
        {
            _context.Wallets.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public async Task<Wallet> AddAsync(Wallet entity)
        {
            await _context.Wallets.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public Wallet Delete(Wallet entity)
        {
            _context.Wallets.Remove(entity);
            _context.SaveChanges();
            return entity;
        }
        public async Task<Wallet> DeleteAsync(Wallet entity)
        {
            _context.Wallets.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public Wallet Get(Expression<Func<Wallet, bool>> predicate)
        {
            return _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .Include(w => w.WalletTransfers)
                .FirstOrDefault(predicate);
        }
        public async Task<Wallet> GetAsync(
        Expression<Func<Wallet, bool>> predicate,
        Func<IQueryable<Wallet>, IQueryable<Wallet>> include = null)
        {
            IQueryable<Wallet> query = _context.Wallets;

            if (include != null)
            {
                query = include(query);
            }
            else
            {
                // İstersen default olarak include edilecek navigation propertyleri ekleyebilirsin:
                query = query.Include(w => w.User)
                             .Include(w => w.Transactions)
                             .Include(w => w.WalletTransfers);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }
        public IEnumerable<Wallet> GetAll(Expression<Func<Wallet, bool>> predicate = null)
        {
            IQueryable<Wallet> query = _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .Include(w => w.WalletTransfers);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query.ToList();
        }
        public async Task<IEnumerable<Wallet>> GetAllAsync(Expression<Func<Wallet, bool>> predicate = null)
        {
            IQueryable<Wallet> query = _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .Include(w => w.WalletTransfers);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync();
        }
        public IQueryable<Wallet> Query()
        {
            return _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .Include(w => w.WalletTransfers);
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        public Wallet Update(Wallet entity)
        {
            _context.Wallets.Update(entity);
            _context.SaveChanges();
            return entity;
        }
        public async Task<Wallet> UpdateAsync(Wallet entity)
        {
            _context.Wallets.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<Wallet>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Wallets
                                 .Where(w => w.UserId == userId)
                                 .Include(w => w.User)
                                 .Include(w => w.Transactions)
                                 .Include(w => w.WalletTransfers)
                                 .ToListAsync();
        }

    }
}
