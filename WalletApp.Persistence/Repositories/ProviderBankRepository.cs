using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;
using System.Data.Entity;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Persistence.Context;

namespace WalletApp.Persistence.Repositories
{
    public class ProviderBankRepository : EfEntityRepositoryBase<ProviderBank>, IProviderBankRepository
    {
        public ProviderBankRepository(WalletDbContext context) : base(context) { }

        public Task<ProviderBank> GetByIdAsync(object providerBankId)
        {
            return _context.ProviderBanks
                .AsNoTracking()
                .FirstOrDefaultAsync(pb => pb.Id == (Guid)providerBankId);
        }

        Task IProviderBankRepository.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}



