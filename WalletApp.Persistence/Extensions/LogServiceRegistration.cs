using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace YourNamespace
{
    public static class LogServiceRegistration
    {
        public static void AddLogService(this IServiceCollection services, IConfiguration configuration)
        {
            var columnOptions = new ColumnOptions
            {
                Store = new Collection<StandardColumn>
                {
                    StandardColumn.TimeStamp
                },
                AdditionalColumns = new List<SqlColumn>
                {
                    new SqlColumn { ColumnName = "UserId", DataType = SqlDbType.NVarChar, DataLength = 100 },
                    new SqlColumn { ColumnName = "Action", DataType = SqlDbType.NVarChar, DataLength = 255 },
                    new SqlColumn { ColumnName = "Method", DataType = SqlDbType.NVarChar, DataLength = 10 },
                    new SqlColumn { ColumnName = "Path", DataType = SqlDbType.NVarChar, DataLength = 255 },
                    new SqlColumn { ColumnName = "StatusCode", DataType = SqlDbType.Int },
                    new SqlColumn { ColumnName = "DurationMs", DataType = SqlDbType.BigInt },
                    new SqlColumn { ColumnName = "IpAddress", DataType = SqlDbType.NVarChar, DataLength = 50 },
                    new SqlColumn { ColumnName = "TraceId", DataType = SqlDbType.NVarChar, DataLength = 50 },
                    new SqlColumn { ColumnName = "RequestBody", DataType = SqlDbType.NVarChar, DataLength = -1 },
                    new SqlColumn { ColumnName = "ResponseBody", DataType = SqlDbType.NVarChar, DataLength = -1 },
                    new SqlColumn { ColumnName = "Description", DataType = SqlDbType.NVarChar, DataLength = -1 },
                    new SqlColumn { ColumnName = "MachineName", DataType = SqlDbType.NVarChar, DataLength = 100 }
                }
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: configuration.GetConnectionString("DefaultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = false
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    columnOptions: columnOptions
                )
                .CreateLogger();

            services.AddSingleton(Log.Logger);
        }
    }
}
