using WalletApp.Domain.Entities;


namespace WalletApp.Application.Abstraction.Repositories
{
    public interface IPaymentRepository : IEntityRepository<AppPayment>
    {
        Task SaveChangesAsync();
    }
}
    