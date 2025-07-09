using Microsoft.EntityFrameworkCore;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Persistence.Base;
using WalletApp.Domain.Base;



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
    }
}

