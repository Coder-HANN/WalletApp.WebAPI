using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.AspNetCore;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.Data;



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

                // Gereksiz standart kolonları kaldır
                columnOptions.Store.Remove(StandardColumn.Properties);
                columnOptions.Store.Remove(StandardColumn.Level);
                columnOptions.Store.Remove(StandardColumn.MessageTemplate);
                columnOptions.Store.Remove(StandardColumn.Exception);
                columnOptions.Store.Remove(StandardColumn.LogEvent);

                // Kullanıcı adımı tablosu için özel kolonlar
                columnOptions.AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn { ColumnName = "UserId", DataType = SqlDbType.NVarChar, DataLength = 100 },
                    new SqlColumn { ColumnName = "Action", DataType = SqlDbType.NVarChar, DataLength = 100 },
                    new SqlColumn { ColumnName = "Description", DataType = SqlDbType.NVarChar, DataLength = 250 },
                    new SqlColumn { ColumnName = "IpAddress", DataType = SqlDbType.NVarChar, DataLength = 50 },
                    new SqlColumn { ColumnName = "RequestTime", DataType = SqlDbType.DateTime2 },
                    new SqlColumn { ColumnName = "DurationMs", DataType = SqlDbType.Int }
                };

                // Logger konfigürasyonu
                loggerConfig
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.MSSqlServer(
                        connectionString: connectionString,
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            TableName = "UserActions",
                            AutoCreateSqlTable = true
                        },
                        columnOptions: columnOptions
                    )
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName();
            });

            return hostBuilder;
        }

        // Kullanıcı adımı loglama methodu
        public static void LogUserAction(
            string userId,
            string action,
            string description = null,
            string ipAddress = null,
            DateTime? requestTime = null,
            int? durationMs = null)
        {
            Log.ForContext("UserId", userId)
               .ForContext("Action", action)
               .ForContext("Description", description ?? string.Empty)
               .ForContext("IpAddress", ipAddress ?? string.Empty)
               .ForContext("RequestTime", requestTime ?? DateTime.UtcNow)
               .ForContext("DurationMs", durationMs ?? 0)
               .Information("User action recorded");
        }
    }
}
