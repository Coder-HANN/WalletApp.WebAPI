
using WalletApp.Domain.Base;



namespace WalletApp.Application.Abstraction.Repositories.EntitysRepository
{
    public interface IUserRepository : IEntityRepository<User>
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
    }

}

