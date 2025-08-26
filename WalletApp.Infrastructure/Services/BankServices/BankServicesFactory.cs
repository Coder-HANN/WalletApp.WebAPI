using WalletApp.Application.Abstraction.Repositories;


namespace WalletApp.Infrastructure.Services.BankServices
{
    public class BankServicesFactory : IBankServicesFactory
    {
        private readonly IBankRouteRepository bankRouteRepository;
        private readonly IProviderBankRepository providerBankRepository;

        public BankServicesFactory(IBankRouteRepository bankRouteRepository, IProviderBankRepository providerBankRepository)
        {
            this.bankRouteRepository = bankRouteRepository;
            this.providerBankRepository = providerBankRepository;
        }
        /// <summary>
        /// Kaynak banka koduna göre 
        /// </summary>
        /// <param name="BankCode"></param>
        /// <returns></returns>
        public IBankServices SelectBankServices(string BankCode)
        {
            return BankCode switch
            {
                "0010" => new ZiraatBankServices(providerBankRepository),
                "0015" => new VakifBankServices(providerBankRepository),
                "0062" => new GarantiBankServices(providerBankRepository),
                _ => new VakifBankServices(providerBankRepository),
            };
        }
    }
}