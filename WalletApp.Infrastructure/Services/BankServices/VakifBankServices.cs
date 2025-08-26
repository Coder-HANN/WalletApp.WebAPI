using MediatR;
using System.Data.Entity;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Infrastructure.Services.BankServices
{
    public class VakifBankServices : IBankServices
    {
        private readonly IProviderBankRepository providerBankRepository;

        public VakifBankServices(IProviderBankRepository providerBankRepository)
        {
            this.providerBankRepository = providerBankRepository;
        }

        public async Task<decimal> BakiyeBilgisi(BankTransferCommand command)
        {
            var selectedProviderBank = await providerBankRepository.Query().FirstOrDefaultAsync(x => x.BankCode == "0015");
          

            if (selectedProviderBank == null)
                throw new Exception("VakifBank provider bilgisi bulunamadı.");

            return selectedProviderBank.TotalBalance;
        }

        public async Task<Unit> ParaTransferi(BankTransferCommand command)
        {
            var selectedProviderBank = await providerBankRepository.Query().FirstOrDefaultAsync(x => x.BankCode == "0015");

            selectedProviderBank.TotalBalance -= command.Amount;
            await providerBankRepository.UpdateAsync(selectedProviderBank);

            return Unit.Value;
        }
    }
}