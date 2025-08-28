using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Domain.Entities;
using WalletApp.Infrastructure.Repositories;
using WalletApp.Infrastructure.Services.BankServices;
using WalletApp.Persistence.Base;
using WalletApp.Persistence.Repositories;


namespace WalletApp.Persistence.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repositories & Services

            services.AddScoped<WalletService>();
            services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserDetailRepository, UserDetailRepository>();
            services.AddScoped<IWalletTransferRepository, WalletTransferRepository>();
            services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
            services.AddScoped<IProviderBankRepository, ProviderBankRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IBankRouteRepository, BankRouteRepository>();
            services.AddScoped(typeof(IEntityRepository<>), typeof(EfEntityRepositoryBase<>));




            return services;
        }
    }
}
