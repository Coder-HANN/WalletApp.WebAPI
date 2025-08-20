using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WalletApp.Application.Abstraction.Services
{
    public interface ILogService
    {
        /// <summary>
        /// Genel log yazımı.
        /// </summary>
        void Log(
            LogLevel level,
            string message,
            Exception? ex = null,
            string? source = null,
            int? userId = null,
            string? requestPath = null,
            object? additionalData = null
        );

        
        void Log(LogLevel logLevel, string message, Exception? exception, string source, Dictionary<string, object> logData);
    }
}
