using WalletApp.Domain.Entities;

namespace WalletApp.Application.Abstraction.Repositories
{
    public interface IProviderBankRepository : IEntityRepository<ProviderBank>
    {
        Task<ProviderBank> GetByIdAsync(Guid providerBankId);
        Task SaveChangesAsync();
    }
}
