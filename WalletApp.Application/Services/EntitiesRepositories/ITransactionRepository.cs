using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Entities;



namespace WalletApp.Application.Services.EntitiesRepositories
{
    public interface ITransactionRepository : IEntityRepository<Transaction>
    {
        Task SaveChangesAsync(Transaction transaction);
        Task SaveChangesAsync();
    }
}
