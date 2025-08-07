using WalletApp.Domain.Entities;

namespace WalletApp.Application.Abstraction.Repositories
{
    public interface IBankRouteRepository : IEntityRepository<BankRoute>
    {
        Task<string> GetProviderBankCodeAsync(string targetBankCode);
    }
}