using WalletApp.Domain.Entities;



namespace WalletApp.Application.Abstraction.Repositories
{
    public interface ITransactionRepository : IEntityRepository<Transaction>
    {
        Task SaveChangesAsync(Transaction transaction);
        Task SaveChangesAsync();
    }
}
