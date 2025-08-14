using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.AspNetCore;
using Microsoft.Extensions.Configuration;


namespace WalletApp.Persistence.Extensions
{
    public static class LogServiceRegistration
    {
        public static IHostBuilder AddLogService(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((ctx, services, loggerConfig) =>
            {
                var connectionString = ctx.Configuration.GetConnectionString("DefaultConnection");

                var columnOptions = new ColumnOptions();
                columnOptions.Store.Remove(StandardColumn.Properties);
                columnOptions.Store.Add(StandardColumn.LogEvent);

                loggerConfig
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.MSSqlServer(
                        connectionString: connectionString,
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            TableName = "Logs",
                            AutoCreateSqlTable = true
                        },
                        columnOptions: columnOptions)
                    .Enrich.FromLogContext();
            });

            return hostBuilder;
        }
    }
}
