using MediatR;
using Quartz;
using System.Data.Entity;
using WalletApp.Application.Abstraction.Repositories;

namespace WalletApp.Application.Abstraction.Services.Jobs
{
    public class ActionServiceJob : IJob
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IActionRepository actionRepository;
        
        public ActionServiceJob(
            ITransactionRepository transactionRepository,
            IWalletRepository walletRepository,
            IActionRepository actionRepository)
        {
            this.transactionRepository = transactionRepository;
            this.walletRepository = walletRepository;
            this.actionRepository = actionRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var transactions = await
                actionRepository
                .Query()
                .Where(_ => _.CreatedDate == DateTime.Today && _.IsTransfer == false)
                .ToListAsync();

            foreach (var transaction in transactions)
            {
                // Remark alanı cüzdan kodu olarak kullanılıyor
                var wallet = await walletRepository.GetAsync(x => x.WalletCode == transaction.Remark);

                if (wallet == null)
                {
                    continue;
                }
                else
                {
                    wallet.TotalBalance += transaction.Amount;
                    await walletRepository.UpdateAsync(wallet);
                    transaction.IsTransfer = true;
                    await actionRepository.UpdateAsync(transaction);
                }
            }
        }
    }
}
