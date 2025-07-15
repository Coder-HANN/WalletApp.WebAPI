using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Base;

namespace WalletApp.Application.Services.Repositories.EntitysRepository
{
    public interface IBankTransactionRepository : IEntityRepository<BankTransaction>
    {
        public interface IProviderBankRepository : IEntityRepository<ProviderBank> { }
    }
}
