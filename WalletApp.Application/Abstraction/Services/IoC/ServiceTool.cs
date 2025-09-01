using Microsoft.Extensions.DependencyInjection;
using System;

namespace WalletApp.Application.Abstraction.Services.IoC
{
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static IServiceProvider Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return ServiceProvider;
        }
    }
}
