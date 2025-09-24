namespace WalletApp.Infrastructure.Services.BankServices
{
    public interface IBankServicesFactory
    {
        IBankServices SelectBankServices(string bankCode);

    }
}
