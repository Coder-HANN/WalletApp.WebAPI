
using WalletApp.Domain.Base;



namespace WalletApp.Application.Abstraction.Repositories.EntitysRepository
{
    public interface IBankTransactionRepository : IEntityRepository<BankTransaction>
    {
		public interface IBankTransectionRepository : IEntityRepository<BankTransaction> { }
		public interface IProviderBankRepository : IEntityRepository<ProviderBank> { }

	}
}
