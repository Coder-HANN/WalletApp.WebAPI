using MediatR;
using System.Data.Entity;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Infrastructure.Services.BankServices
{
    public class ZiraatBankServices : IBankServices
    {
        private readonly IProviderBankRepository providerBankRepository;
        public ZiraatBankServices(IProviderBankRepository providerBankRepository)
        {
            this.providerBankRepository = providerBankRepository;
        }

        public async Task<decimal> BakiyeBilgisi(BankTransferCommand command)
        {
            var selectedProviderBank = await providerBankRepository.Query().FirstOrDefaultAsync(x => x.BankCode == "0010");
            
            if (selectedProviderBank == null)
                throw new Exception("Ziraat Bank provider bilgisi bulunamadı.");

            if (selectedProviderBank.TotalBalance < command.Amount)
                throw new Exception("Ziraat Bank bakiyesi yetersiz.");

            return selectedProviderBank.TotalBalance;
        }

        public async Task<Unit> ParaTransferi(BankTransferCommand command)
        {
            var selectedProviderBank = await providerBankRepository.Query().FirstOrDefaultAsync(x => x.BankCode == "0010");

            selectedProviderBank.TotalBalance -= command.Amount;
            await providerBankRepository.UpdateAsync(selectedProviderBank);

            return Unit.Value;
        }
    }
}
