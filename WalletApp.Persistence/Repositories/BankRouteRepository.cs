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

    public Task<string> GetProviderBankCodeAsync(string targetBankCode)
    {
        throw new NotImplementedException();
    }

    public class VakifBankCode 
    {
        public const string Code = "0015";
    }
    public class ZiraatBankCode
    {
        public const string Code = "0010";
    }
    public class GarantiBankCode
    {
        public const string Code = "0020";
    }

}
