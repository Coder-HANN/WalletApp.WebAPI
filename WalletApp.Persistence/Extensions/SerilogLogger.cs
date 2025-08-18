using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;
using System.Collections.Generic;

public class SerilogLogger : ILogService
{
    private readonly ILogger _logger;

    public SerilogLogger(IConfiguration configuration)
    {
        var serilogConfig = configuration.GetSection("LoggingConfig:Providers:Serilog");
        if (!serilogConfig.GetValue<bool>("Enabled"))
            return;

        _logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    public void Log(LogLevel level, string message, Exception? ex = null, string? source = null, int? userId = null, string? requestPath = null, object? additionalData = null)
    {
        // additionalData'yi dictionary olarak kullan
        var data = additionalData as IDictionary<string, object> ?? new Dictionary<string, object>();

        var logEvent = new
        {
            Level = level.ToString(),
            Message = message,
            Exception = ex?.ToString(),
            RequestPath = requestPath,
            UserId = userId ?? (data.ContainsKey("UserId") ? data["UserId"]  : null),
            RequestBody = data.ContainsKey("RequestBody") ? data["RequestBody"] : null,
            ResponseBody = data.ContainsKey("ResponseBody") ? data["ResponseBody"] : null,
            StatusCode = data.ContainsKey("StatusCode") ? data["StatusCode"] : null,
            IpAddress = data.ContainsKey("IpAddress") ? data["IpAddress"] : null,
            MachineName = Environment.MachineName,
            RequestTime = data.ContainsKey("RequestTime") ? data["RequestTime"] : DateTime.UtcNow,
            DurationMs = data.ContainsKey("DurationMs") ? data["DurationMs"] : 0
        };

        switch (level)
        {
            case LogLevel.Trace: _logger?.Verbose("{@LogEvent}", logEvent); break;
            case LogLevel.Debug: _logger?.Debug("{@LogEvent}", logEvent); break;
            case LogLevel.Information: _logger?.Information("{@LogEvent}", logEvent); break;
            case LogLevel.Warning: _logger?.Warning("{@LogEvent}", logEvent); break;
            case LogLevel.Error: _logger?.Error(ex, "{@LogEvent}", logEvent); break;
            case LogLevel.Critical: _logger?.Fatal(ex, "{@LogEvent}", logEvent); break;
        }
    }

   
}
