using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Persistence.Base;
using WalletApp.Persistence.Context;
using Action = WalletApp.Domain.Entities.Action;

namespace WalletApp.Persistence.Repositories
{
    public class ActionRepository : EfEntityRepositoryBase<Action>, IActionRepository
    {
        public ActionRepository(WalletDbContext context) : base(context)
        {
            
        }

        //public async Task<Action> GetTransferActionAsync(bool IsTransfer)
        //{
        //    return await _context.Actions
        //        .Where(a => a.IsTransfer == IsTransfer)
        //        .AsQueryable();
        //}
    }
}
