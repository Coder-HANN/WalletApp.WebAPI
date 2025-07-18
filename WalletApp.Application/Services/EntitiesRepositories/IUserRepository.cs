
using System.Linq.Expressions;
using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Entities;



namespace WalletApp.Application.Services.EntitiesRepositories
{
    public interface IUserRepository : IEntityRepository<AppUser>
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
        Task<AppUser> GetAsync(Expression<Func<AppUser, bool>> predicate, Func<IQueryable<AppUser>, IQueryable<AppUser>> include = null);

    }

}

