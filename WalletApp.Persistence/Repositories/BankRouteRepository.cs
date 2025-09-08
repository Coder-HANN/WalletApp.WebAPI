using MediatR;
using System.Data.Entity;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence.Base;
using WalletApp.Persistence.Context;


public class BankRouteRepository : EfEntityRepositoryBase<BankRoute>, IBankRouteRepository
{
    private readonly WalletDbContext context;
    public BankRouteRepository(WalletDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<Guid> GetProviderBankIdAsync(Guid targetBankId)
    {
        if (targetBankId == Guid.Empty)
        {
            _context.BankRoutes
            .Where(x => x.TargetBankId == null)
            .Select(x => x.SourceBankId)
            .FirstOrDefaultAsync();
        } else
        {
           _context.BankRoutes
           .Where(x => x.TargetBankId == targetBankId)
           .Select(x => x.SourceBankId)
           .FirstOrDefaultAsync();
        }
        return Task.FromResult(Guid.Empty);
    }
}
