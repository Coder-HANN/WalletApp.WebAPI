using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence.Context;

namespace WalletApp.Persistence.Base
{
    public class WalletTransferRepository : EfEntityRepositoryBase<WalletTransfer>, IWalletTransferRepository
    {

        public WalletTransferRepository(WalletDbContext context) : base(context) {}

        public async Task<WalletTransfer> AddAsync(WalletTransfer entity)
        {
            await _context.WalletTransfers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public WalletTransfer Add(WalletTransfer entity)
        {
            _context.WalletTransfers.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public WalletTransfer Delete(WalletTransfer entity)
        {
            _context.WalletTransfers.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<WalletTransfer> DeleteAsync(WalletTransfer entity)
        {
            _context.WalletTransfers.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public WalletTransfer Get(Expression<Func<WalletTransfer, bool>> predicate)
        {
            return _context.WalletTransfers.FirstOrDefault(predicate);
        }

        public IEnumerable<WalletTransfer> GetAll(Expression<Func<WalletTransfer, bool>> predicate = null)
        {
            return predicate == null
                ? _context.WalletTransfers
                : _context.WalletTransfers.Where(predicate);
        }

        public async Task<IEnumerable<WalletTransfer>> GetAllAsync(Expression<Func<WalletTransfer, bool>> predicate = null)
        {
            return predicate == null
                ? await _context.WalletTransfers.ToListAsync()
                : await _context.WalletTransfers.Where(predicate).ToListAsync();
        }

        public async Task<WalletTransfer> GetAsync(
            Expression<Func<WalletTransfer, bool>> predicate,
            Func<IQueryable<WalletTransfer>, IQueryable<WalletTransfer>> include = null)
        {
            IQueryable<WalletTransfer> query = _context.WalletTransfers;

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<WalletTransfer> Query()
        {
            return _context.WalletTransfers.AsQueryable();
        }

        public WalletTransfer Update(WalletTransfer entity)
        {
            _context.WalletTransfers.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<WalletTransfer> UpdateAsync(WalletTransfer entity)
        {
            _context.WalletTransfers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<WalletTransfer?> GetByIdAsync(Guid id)
        {
            return await _context.WalletTransfers.FindAsync(id);
        }

      
    }
}
