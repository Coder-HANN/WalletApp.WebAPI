using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence.Context;
using WalletApp.Persistence.Repositories;


namespace WalletApp.Persistence.Base
{
    public class EfEntityRepositoryBase<T> : IEntityRepository<T> where T : class
    {
        protected WalletDbContext _context;
        protected DbSet<T> _dbSet;

        public EfEntityRepositoryBase(WalletDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public T? GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }
        public T Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public T Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public T Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
            return entity;
        }
        public async Task<T> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }
        public async Task<T> GetAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(predicate);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet.ToList();
            }
            return _dbSet.Where(predicate).ToList();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.ToListAsync();
            }
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<IPagingExecutionResult<T>> GetPagedResult<T>(IEnumerable<T> query,int? pageSize = 10,int? pageIndex = 1,Func<IQueryable<T>, IOrderedQueryable<T>> ordering = default,
            CancellationToken cancellationToken = default)
        {
            if ((pageIndex ??= 1) < 1) pageIndex = 1;
            if ((pageSize ??= 10) < 1) pageSize = 1;

            var hasPaging = false;
            var totalCount = 0;

            if (pageSize.HasValue && pageIndex.HasValue)
            {
                hasPaging = true;
                totalCount = query.Count();
                query = ordering == null? query: ordering(query.AsQueryable());
                query = query.Skip(pageSize.Value * (pageIndex.Value - 1)).Take(pageSize.Value);
            }

            var data = query.ToList();

            return Task.FromResult<IPagingExecutionResult<T>>(new PagingExecutionResult<T>(data, hasPaging, pageIndex.Value, pageSize.Value, totalCount)
            );
        }

    }
}