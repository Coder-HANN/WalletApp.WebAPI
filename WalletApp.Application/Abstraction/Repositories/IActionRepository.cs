using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = WalletApp.Domain.Entities.Action;
namespace WalletApp.Application.Abstraction.Repositories
{
    public interface IActionRepository : IEntityRepository<Action> {}
}
