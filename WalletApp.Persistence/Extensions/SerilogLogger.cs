using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using WalletApp.Application.Abstraction.Services;

namespace WalletApp.Infrastructure.Logging
{
    /// <summary>
    /// Serilog ile ILogService adaptörü.
    /// </summary>
    public class SerilogLogger : ILogService
    {
        private readonly Serilog.ILogger _logger;
        private readonly ICurrentUserService _currentUserService;

        public SerilogLogger(Serilog.ILogger logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
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

            int? effectiveUserId = userId;
            try
            {
                effectiveUserId ??= _currentUserService.CurrentUser();
            }
            catch
            {
                // authenticated değilse null kalır
            }

            var log = _logger
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

            switch (level)
            {
                case LogLevel.Trace: log.Verbose(message); break;
                case LogLevel.Debug: log.Debug(message); break;
                case LogLevel.Information: log.Information(message); break;
                case LogLevel.Warning: log.Warning(message); break;
                case LogLevel.Error: log.Error(ex, message); break;
                case LogLevel.Critical: log.Fatal(ex, message); break;
                case LogLevel.None: log.Information(message); break;
            }
        }

        public void Log(LogLevel logLevel, string message, Exception? exception, string source, Dictionary<string, object> logData)
            => Log(logLevel, message, exception, source, null, null, logData);
    }
}
