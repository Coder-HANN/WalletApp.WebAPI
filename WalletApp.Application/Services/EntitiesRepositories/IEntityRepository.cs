using System.Linq.Expressions;


namespace WalletApp.Application.Services.Repositories
{
    public interface IEntityRepository<T> where T : class
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
        T Update(T entity);
        Task<T> UpdateAsync(T entity);
        T Delete(T entity);
        Task<T> DeleteAsync(T entity);
        T Get(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IQueryable<T>> include = null);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
        IQueryable<T> Query();
        Task<int> SaveChangesAsync();
       
        Task<T?> GetByIdAsync(Guid id);
        
    }
}
