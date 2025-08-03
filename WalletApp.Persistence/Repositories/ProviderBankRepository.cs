using WalletApp.Domain.Entities;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Persistence.Base;
using System.Data.Entity;

namespace WalletApp.Persistence.Repositories
{
    public class ProviderBankRepository : EfEntityRepositoryBase<ProviderBank>, IProviderBankRepository
    {
        public ProviderBankRepository(WalletDbContext context) : base(context) { }

        public async Task<IEnumerable<ProviderBank>> GetByUserIdAsync(int currentUserId)
        {
            return await _context.ProviderBanks
                .Where(x => x.AppUserId == currentUserId && !x.IsDelete) // IsDelete kontrolü opsiyonel ama genelde silinmiş hesaplar filtrelenir
                .ToListAsync();
        }

        public Task GetProviderBankAsync(int currentUserId)
        {
            throw new NotImplementedException();
        }

        Task IProviderBankRepository.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}



