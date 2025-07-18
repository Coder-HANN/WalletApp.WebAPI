using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
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

        public async Task<AppWallet?> GetByIdAsync(Guid id)
        {
            return await _context.Wallets.FindAsync(id);
        }

        public AppWallet Add(AppWallet entity)
        {
            _context.Wallets.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<AppWallet> AddAsync(AppWallet entity)
        {
            await _context.Wallets.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public AppWallet Delete(AppWallet entity)
        {
            _context.Wallets.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<AppWallet> DeleteAsync(AppWallet entity)
        {
            _context.Wallets.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public AppWallet Get(Expression<Func<AppWallet, bool>> predicate)
        {
            return _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .Include(w => w.WalletTransfers)
                .FirstOrDefault(predicate);
        }

        public async Task<AppWallet> GetAsync(
            Expression<Func<AppWallet, bool>> predicate,
            Func<IQueryable<AppWallet>, IQueryable<AppWallet>> include = null)
        {
            IQueryable<AppWallet> query = _context.Wallets;

            if (include != null)
            {
                query = include(query);
            }
            else
            {
                query = query.Include(w => w.User)
                             .Include(w => w.Transactions)
                             .Include(w => w.WalletTransfers);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<AppWallet> GetAll(Expression<Func<AppWallet, bool>> predicate = null)
        {
            IQueryable<AppWallet> query = _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .Include(w => w.WalletTransfers);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query.ToList();
        }

        public async Task<IEnumerable<AppWallet>> GetAllAsync(Expression<Func<AppWallet, bool>> predicate = null)
        {
            IQueryable<AppWallet> query = _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .Include(w => w.WalletTransfers);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync();
        }

        public IQueryable<AppWallet> Query()
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

        public AppWallet Update(AppWallet entity)
        {
            _context.Wallets.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<AppWallet> UpdateAsync(AppWallet entity)
        {
            _context.Wallets.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<AppWallet>> GetAllByAppUserIdAsync(int AppUserId)
        {
            return await _context.Wallets
                                 .Where(w => w.AppUserId == AppUserId)
                                 .Include(w => w.AppUserId)
                                 .Include(w => w.Transactions)
                                 .Include(w => w.WalletTransfers)
                                 .ToListAsync();
        }

    }
}
