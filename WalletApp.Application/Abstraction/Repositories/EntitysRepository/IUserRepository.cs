
using System.Linq.Expressions;
using WalletApp.Domain.Base;



namespace WalletApp.Application.Abstraction.Repositories.EntitysRepository
{
    public interface IUserRepository : IEntityRepository<User>
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
        Task<User> GetAsync(Expression<Func<User, bool>> predicate, Func<IQueryable<User>, IQueryable<User>> include = null);

    }

}

