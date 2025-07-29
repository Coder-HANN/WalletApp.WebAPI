using WalletApp.Domain.Entities;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Persistence.Base;

namespace WalletApp.Persistence.Repositories
{
    public class ProviderBankRepository : EfEntityRepositoryBase<ProviderBank>, IProviderBankRepository
    {
        public ProviderBankRepository(WalletDbContext context) : base(context) { }
    }
}



