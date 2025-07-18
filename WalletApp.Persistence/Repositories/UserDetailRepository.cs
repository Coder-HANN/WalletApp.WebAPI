

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
    }
}
