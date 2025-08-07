using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.WebAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            context.Request.EnableBuffering();

            var requestBody = string.Empty;
            if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
            {
                using var reader = new StreamReader(
                    context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                try
                {
                    LogContext.PushProperty("Path", context.Request.Path);
                    LogContext.PushProperty("Method", context.Request.Method);
                    LogContext.PushProperty("StatusCode", context.Response.StatusCode);
                    LogContext.PushProperty("RequestBody", requestBody);
                    LogContext.PushProperty("ResponseBody", responseText);
                    LogContext.PushProperty("IpAddress", context.Connection.RemoteIpAddress?.ToString());
                    LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"].ToString());
                    LogContext.PushProperty("ResponseTimeMs", stopwatch.ElapsedMilliseconds);

                    var userId = context.User?.Claims?.FirstOrDefault(c => c.Type == "uid")?.Value;
                    LogContext.PushProperty("UserId", userId ?? "Anonymous");
                    LogContext.PushProperty("TraceId", context.TraceIdentifier);

                    Log.Information("HTTP {Method} {Path} responded {StatusCode} in {ResponseTimeMs} ms");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Log kaydı alınırken hata oluştu.");
                }

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}
