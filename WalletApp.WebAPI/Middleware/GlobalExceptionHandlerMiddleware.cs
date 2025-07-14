using WalletApp.Application.DTO;

namespace WalletApp.WebAPI.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu.");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var response = new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Sunucu hatası: " + ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
