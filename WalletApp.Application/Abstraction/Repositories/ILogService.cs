using Microsoft.Extensions.Logging;
using System;

public interface ILogService
{
    /// <summary>
    /// Log mesajını yazar.
    /// </summary>
    /// <param name="level">Log seviyesi (Information, Error, vb.)</param>
    /// <param name="message">Log mesajı</param>
    /// <param name="ex">Exception (opsiyonel)</param>
    /// <param name="source">Log kaynağı (opsiyonel)</param>
    /// <param name="userId">Kullanıcı Id (opsiyonel)</param>
    /// <param name="requestPath">İstek yolu (opsiyonel)</param>
    /// <param name="additionalData">Ek veri (RequestBody, ResponseBody, StatusCode, vb.)</param>
    void Log(
        LogLevel level,
        string message,
        Exception? ex = null,
        string? source = null,
        int? userId = null,
        string? requestPath = null,
        object? additionalData = null
    );
}