using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Logging.Models;
using WalletApp.Persistence.Context;

namespace WalletApp.Logging.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestTime = DateTime.UtcNow;
            var stopwatch = Stopwatch.StartNew();

            // --- Request Body ---
            context.Request.EnableBuffering();
            string requestBody = "";
            if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
            {
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            // --- Response Body ---
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            Exception? exception = null;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                exception = ex;
                context.Response.StatusCode = 500;
            }
            finally
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                stopwatch.Stop();

                // --- Log to DB ---
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

                var log = new ApplicationLog
                {
                    Level = exception != null ? "Error" : "Info",
                    Message = exception?.Message ?? "HTTP Request completed",
                    Exception = exception?.ToString() ?? "", // NULL yerine boş string
                    RequestPath = context.Request.Path,
                    RequestBody = requestBody,
                    ResponseBody = responseBodyText,
                    StatusCode = context.Response.StatusCode,
                    IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                    MachineName = System.Environment.MachineName,
                    RequestTime = requestTime,
                    DurationMs = stopwatch.ElapsedMilliseconds
                };

                dbContext.Set<ApplicationLog>().Add(log);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
