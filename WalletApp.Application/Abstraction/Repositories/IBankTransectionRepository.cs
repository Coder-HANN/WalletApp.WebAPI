using WalletApp.Domain.Entities;

namespace WalletApp.Application.Abstraction.Repositories
{
    public interface IBankTransactionRepository : IEntityRepository<BankTransaction>
    {
        public interface IProviderBankRepository : IEntityRepository<ProviderBank> { }
    }
}
