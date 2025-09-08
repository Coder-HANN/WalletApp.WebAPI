using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.Abstraction.Services.Transaction;
using WalletApp.Application.Feature.Wallet.Handlers;

namespace WalletApp.Persistence.Extensions
{
    public static class TransactionServiceCollectionExtensions
    {
        public static IServiceCollection AddTransactionServices(this IServiceCollection services)
        {
            services.AddTransient<WalletService>(provider =>
            {
                var transactionService = provider.GetRequiredService<ITransactionService>();
                var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();

                var walletRepository = provider.GetRequiredService<IWalletRepository>();
                var transactionRepository = provider.GetRequiredService<ITransactionRepository>();
                var walletTransferRepository = provider.GetRequiredService<IWalletTransferRepository>();
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var providerBankRepository = provider.GetRequiredService<IProviderBankRepository>();
                var bankTransactionRepository = provider.GetRequiredService<IBankTransactionRepository>();
                var currentUserService = provider.GetRequiredService<ICurrentUserService>();

                var walletService = new WalletService(
                    walletRepository,
                    transactionRepository,
                    walletTransferRepository,
                    httpContextAccessor,
                    providerBankRepository,
                    bankTransactionRepository,
                    currentUserService
                );

                return proxyGenerator.CreateClassProxyWithTarget(
                    walletService,
                    new TransactionAspect(transactionService)
                );
            });

            return services;
        }
    }
}