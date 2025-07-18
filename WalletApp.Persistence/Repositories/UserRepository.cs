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
    }
}

