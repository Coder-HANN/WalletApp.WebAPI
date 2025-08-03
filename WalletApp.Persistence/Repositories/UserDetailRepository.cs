

using System.Data.Entity;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;

namespace WalletApp.Persistence.Repositories
{
    public class UserDetailRepository : EfEntityRepositoryBase<UserDetail>, IUserDetailRepository
    {
        public UserDetailRepository(WalletDbContext context ) : base(context)
        {
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        Task IUserDetailRepository.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}
