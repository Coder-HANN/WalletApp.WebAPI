using Serilog.Sinks.MSSqlServer;
using System.Collections.Generic;
using System.Data;

namespace WalletApp.Infrastructure.Logging
{
    public static class LoggingColumns
    {
        // Kolonları tek yerde yönetiyoruz. Buradaki şema DB tablosunu otomatik oluştururken de kullanılacak.
        public static ColumnOptions GetColumnOptions()
        {
            return new ColumnOptions
            {
                AdditionalColumns = new List<SqlColumn>
                {
                    new SqlColumn("Source",       SqlDbType.NVarChar) { DataLength = 256,  AllowNull = true },
                    new SqlColumn("UserId",       SqlDbType.Int)      {                    AllowNull = true },
                    new SqlColumn("RequestPath",  SqlDbType.NVarChar) { DataLength = 512,  AllowNull = true },
                    new SqlColumn("RequestBody",  SqlDbType.NVarChar) { DataLength = -1,   AllowNull = true },
                    new SqlColumn("ResponseBody", SqlDbType.NVarChar) { DataLength = -1,   AllowNull = true },
                    new SqlColumn("StatusCode",   SqlDbType.Int)      {                    AllowNull = true },
                    new SqlColumn("IpAddress",    SqlDbType.NVarChar) { DataLength = 64,   AllowNull = true },
                    new SqlColumn("MachineName",  SqlDbType.NVarChar) { DataLength = 128,  AllowNull = true },
                    new SqlColumn("RequestTime",  SqlDbType.DateTime) {                    AllowNull = true },
                    new SqlColumn("DurationMs",   SqlDbType.BigInt)   {                    AllowNull = true },
                }
            };
        }
    }
}
