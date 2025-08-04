using Microsoft.EntityFrameworkCore;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;

namespace WalletApp.Persistence.Repositories
{
    public class UserDetailRepository : EfEntityRepositoryBase<UserDetail>, IUserDetailRepository
    {
        private readonly WalletDbContext _context;

        public UserDetailRepository(WalletDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
