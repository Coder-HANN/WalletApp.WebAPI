using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        stopwatch.Stop();

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var userId = context.User?.Identity?.Name ?? "Anonymous";
        var action = context.GetEndpoint()?.DisplayName ?? "Unknown";

        Log.Information("{Timestamp} {UserId} {Action} {Method} {Path} {StatusCode} {DurationMs} {IpAddress} {TraceId} {RequestBody} {ResponseBody} {Description} {MachineName}",
            DateTime.Now,
            userId,
            action,
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            context.Connection.RemoteIpAddress?.ToString(),
            context.TraceIdentifier,
            requestBody,
            responseBodyText,
            "Custom Description",
            Environment.MachineName
        );

        await responseBody.CopyToAsync(originalBodyStream);
    }
}
