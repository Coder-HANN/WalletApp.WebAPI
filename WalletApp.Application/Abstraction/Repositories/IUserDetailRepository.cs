using WalletApp.Domain.Entities;



namespace WalletApp.Application.Abstraction.Repositories
{
    public interface IUserDetailRepository : IEntityRepository<UserDetail>
    {
        Task<bool> ExistsAsync(string email);
        Task SaveChangesAsync();
    }
}
