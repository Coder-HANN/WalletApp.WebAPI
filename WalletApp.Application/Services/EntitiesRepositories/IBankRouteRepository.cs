using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Services.EntitiesRepositories
{
    public interface IBankRouteRepository : IEntityRepository<BankRoute>
    {
        Task<string> GetProviderBankCodeAsync(string targetBankCode);
    }
}