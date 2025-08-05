using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Persistence;
using WalletApp.Persistence.Base;

public class BankRouteRepository : EfEntityRepositoryBase<BankRoute>, IBankRouteRepository
{
    private readonly WalletDbContext context;
    public BankRouteRepository(WalletDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<string> GetProviderBankCodeAsync(string targetBankCode)
    {
        string providerBankCode = targetBankCode switch
        {
            "0010" => "0010", // Ziraat → Ziraat
            "0015" => "0015", // Vakıf → Vakıf
            "0020" => "0010", // Garanti → Ziraat
            _ => "0015"       // Varsayılan: Vakıf
        };

        return Task.FromResult(providerBankCode);
    }
}
