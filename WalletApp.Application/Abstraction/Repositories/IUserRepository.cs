
using System.Linq.Expressions;
using WalletApp.Domain.Entities;



namespace WalletApp.Application.Abstraction.Repositories
{
    public interface IUserRepository : IEntityRepository<AppUser>
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
        Task<AppUser> GetAsync(Expression<Func<AppUser, bool>> predicate, Func<IQueryable<AppUser>, IQueryable<AppUser>> include = null);
        Task<int> SaveChangesAsync();
        Task DeleteAsync(AppUser user);
        Task<AppUser> GetByEmailAsync(string email);
        Task AddUserAsync(AppUser user);
        Task<AppUser> GetByUserIdAsync(int userId);
    }

}

