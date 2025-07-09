using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
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
                .Include(w => w.Transections)
                .Include(w => w.WalletTransfers)
                .FirstOrDefault(predicate);
        }

        public async Task<Wallet> GetAsync(Expression<Func<Wallet, bool>> predicate)
        {
            return await _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transections)
                .Include(w => w.WalletTransfers)
                .FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<Wallet> GetAll(Expression<Func<Wallet, bool>> predicate = null)
        {
            IQueryable<Wallet> query = _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transections)
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
                .Include(w => w.Transections)
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
                .Include(w => w.Transections)
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
    }
}
