using MediatR;
using System.Data.Entity;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Infrastructure.Services.BankServices
{
    public class GarantiBankServices : IBankServices
    {
        private readonly IProviderBankRepository providerBankRepository;
        public GarantiBankServices(IProviderBankRepository providerBankRepository)
        {
            this.providerBankRepository = providerBankRepository;
        }
        public async Task<decimal> BakiyeBilgisi(BankTransferCommand command)
        {
            var selectedProviderBank = await providerBankRepository.Query().FirstOrDefaultAsync(x => x.BankCode == "0062");
            

            if (selectedProviderBank != null)
                throw new Exception("Provider Banka bulunamadı");

            if (selectedProviderBank.TotalBalance < command.Amount)
                throw new Exception("Yeterli bakiye yok");

            return selectedProviderBank.TotalBalance;
        }

        public async Task<Unit> ParaTransferi(BankTransferCommand command)
        {
            var selectedProviderBank = await providerBankRepository.Query().FirstOrDefaultAsync(x => x.BankCode == "0062");

            selectedProviderBank.TotalBalance -= command.Amount;
            providerBankRepository.UpdateAsync(selectedProviderBank);

            return Unit.Value;
            
        }
    }
}
