using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

public class CompositeLogger : ILogService
{
    private readonly List<ILogService> _loggers = new();

    public CompositeLogger(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var serilogEnabled = configuration.GetValue<bool>("LoggingConfig:Providers:Serilog:Enabled");
        if (serilogEnabled)
        {
            // SerilogLogger'ı DI ile kayıt ettiğimiz için alıyoruz.
            _loggers.Add(serviceProvider.GetRequiredService<SerilogLogger>());
        }
        // ileride başka logger eklenebilir
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
        foreach (var logger in _loggers)
        {
            try { logger.Log(level, message, ex, source, userId, requestPath, additionalData); }
            catch { /* bir logger başarısız olsa diğerleri yine çalışsın */ }
        }
    }
}
