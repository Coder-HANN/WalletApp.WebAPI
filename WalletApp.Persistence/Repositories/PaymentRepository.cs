using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;
using WalletApp.Persistence.Context;

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
