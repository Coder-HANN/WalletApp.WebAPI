using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Services.EntitiesRepositories
{
    public interface IBankTransactionRepository : IEntityRepository<BankTransaction>
    {
        public interface IProviderBankRepository : IEntityRepository<ProviderBank> { }
    }
}
