using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Domain.Base;
using WalletApp.Persistence.Base;




namespace WalletApp.Persistence.Repositories

{
    public class UserRepository : EfEntityRepositoryBase<User>, IUserRepository
    {
        public UserRepository(WalletDbContext context) : base(context)
        {
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
        }
        public async Task<User> GetAsync(Expression<Func<User, bool>> predicate, Func<IQueryable<User>, IQueryable<User>> include = null)
        {
            IQueryable<User> query = _context.Users;

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync();
        }
    }
}

