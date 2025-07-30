using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;

namespace WalletApp.Persistence.Repositories
{
    public class PaymentRepository : EfEntityRepositoryBase<AppPayment>, IPaymentRepository
    {
        public PaymentRepository(WalletDbContext context) : base(context)
        {
        }

        Task IPaymentRepository.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}
