using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WalletApp.Application.Abstraction.Services;

namespace WalletApp.Infrastructure.Logging
{
    /// <summary>
    /// Birden fazla log sağlayıcısını kompoze etmek için basit bir orkestratör.
    /// Şimdilik Serilog var; ileride başka sağlayıcı kolayca eklenebilir.
    /// </summary>
    public class CompositeLogger : ILogService
    {
        private readonly IServiceProvider _serviceProvider;

        public CompositeLogger(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Log(
            LogLevel level,
            string message,
            Exception? ex = null,
            string? source = null,
            int? userId = null,
            string? requestPath = null,
            object? additionalData = null)
        {
            using var scope = _serviceProvider.CreateScope();

            var loggers = new List<ILogService>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var serilogEnabled = configuration.GetValue<bool>("LoggingConfig:Providers:Serilog:Enabled");
            if (serilogEnabled)
            {
                var serilogLogger = scope.ServiceProvider.GetRequiredService<SerilogLogger>();
                loggers.Add(serilogLogger);
            }

            // ileride: if (nlogEnabled) loggers.Add(scope.ServiceProvider.GetRequiredService<NLogLogger>());

            foreach (var logger in loggers)
            {
                try
                {
                    logger.Log(level, message, ex, source, userId, requestPath, additionalData);
                }
                catch
                {
                    // bir sağlayıcı hata verse bile diğerleri yazmaya devam etsin
                }
            }
        }

        public void Log(LogLevel logLevel, string message, Exception? exception, string source, Dictionary<string, object> logData)
            => Log(logLevel, message, exception, source, null, null, logData);
    }
}
