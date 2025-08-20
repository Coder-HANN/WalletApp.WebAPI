using System.Diagnostics;
using System.Text;
using WalletApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Http;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;


    public RequestResponseLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task Invoke(HttpContext context, ICurrentUserService currentUserService, ILogService logger)
    {
        // UserId al
        var userId = currentUserService?.CurrentUser() ?? 0;

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
            stopwatch.Stop();

            // ResponseBody oku
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);

            // --- Log gönder ---
            logger.Log(
                level: exception != null ? LogLevel.Error : LogLevel.Information,
                message: exception?.Message ?? "HTTP Request completed",
                ex: exception,
                requestPath: context.Request.Path,
                userId: userId,
                additionalData: new Dictionary<string, object>
                {
                { "RequestBody", string.IsNullOrWhiteSpace(requestBody) ? null : requestBody },
                { "ResponseBody", string.IsNullOrWhiteSpace(responseBodyText) ? null : responseBodyText },
                { "StatusCode", context.Response.StatusCode },
                { "IpAddress", context.Connection.RemoteIpAddress?.ToString() },
                { "RequestTime", requestTime },
                { "DurationMs", stopwatch.ElapsedMilliseconds }
                }
            );
        }
    }

}
