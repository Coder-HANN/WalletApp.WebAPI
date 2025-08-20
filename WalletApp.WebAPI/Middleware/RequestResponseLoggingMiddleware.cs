using System.Diagnostics;
using System.Text;
using WalletApp.Application.Abstraction.Services;

namespace WalletApp.WebAPI.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ICurrentUserService currentUserService,
            ILogService logger)
        {
            int? userId = null;
            try
            {
                userId = currentUserService.CurrentUser();
            }
            catch { /* anonim istek olabilir */ }

            var requestTime = DateTime.UtcNow;
            var sw = Stopwatch.StartNew();

            // Request body
            context.Request.EnableBuffering();
            string requestBody = string.Empty;
            if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
            {
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 1024, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            // Response yakalama
            var originalBodyStream = context.Response.Body;
            await using var responseBuffer = new MemoryStream();
            context.Response.Body = responseBuffer;

            Exception? exception = null;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                exception = ex;
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            finally
            {
                sw.Stop();

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // Response'u geri kopyala
                await responseBuffer.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;

                var logData = new Dictionary<string, object>
                {
                    { "RequestPath", context.Request.Path.ToString() },
                    { "RequestBody", string.IsNullOrWhiteSpace(requestBody) ? null : requestBody },
                    { "ResponseBody", string.IsNullOrWhiteSpace(responseBodyText) ? null : responseBodyText },
                    { "StatusCode", context.Response.StatusCode },
                    { "IpAddress", context.Connection.RemoteIpAddress?.ToString() },
                    { "RequestTime", requestTime },
                    { "DurationMs", sw.ElapsedMilliseconds },
                    { "UserId", userId.HasValue ? userId.Value : (int?)null }
                };

                logger.Log(
                    exception != null ? LogLevel.Error : LogLevel.Information,
                    "HTTP request",
                    exception,
                    "RequestResponse",
                    logData
                );
            }
        }
    }
}
