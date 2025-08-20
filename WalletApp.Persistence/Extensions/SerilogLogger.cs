using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WalletApp.Application.Abstraction.Services;
using System;
using System.Collections.Generic;

public class SerilogLogger : ILogService
{
    private readonly Serilog.ILogger _logger;
    private readonly ICurrentUserService _currentUserService;

    public SerilogLogger(IConfiguration configuration, ICurrentUserService currentUserService)
    {
        _logger = Serilog.Log.Logger;
        _currentUserService = currentUserService;
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
        var data = additionalData as IDictionary<string, object> ?? new Dictionary<string, object>();

        data.TryGetValue("RequestPath", out var reqPath);
        data.TryGetValue("RequestBody", out var reqBody);
        data.TryGetValue("ResponseBody", out var resBody);
        data.TryGetValue("StatusCode", out var statusCode);
        data.TryGetValue("IpAddress", out var ip);
        data.TryGetValue("RequestTime", out var reqTime);
        data.TryGetValue("DurationMs", out var durationMs);

        // CurrentUserService’den userId al
        int effectiveUserId = _currentUserService.CurrentUser();

        var logger = _logger
            .ForContext("Source", source ?? string.Empty)
            .ForContext("UserId", effectiveUserId)
            .ForContext("RequestPath", requestPath ?? reqPath)
            .ForContext("RequestBody", reqBody)
            .ForContext("ResponseBody", resBody)
            .ForContext("StatusCode", statusCode)
            .ForContext("IpAddress", ip)
            .ForContext("MachineName", Environment.MachineName)
            .ForContext("RequestTime", reqTime ?? DateTime.UtcNow)
            .ForContext("DurationMs", durationMs ?? 0);

        var logMessage = message ?? (ex != null ? ex.Message : "HTTP Request completed");

        switch (level)
        {
            case LogLevel.Trace:
                logger.Verbose(logMessage);
                break;
            case LogLevel.Debug:
                logger.Debug(logMessage);
                break;
            case LogLevel.Information:
                logger.Information(logMessage);
                break;
            case LogLevel.Warning:
                logger.Warning(logMessage);
                break;
            case LogLevel.Error:
                logger.Error(ex, logMessage);
                break;
            case LogLevel.Critical:
                logger.Fatal(ex, logMessage);
                break;
        }
    }
}
