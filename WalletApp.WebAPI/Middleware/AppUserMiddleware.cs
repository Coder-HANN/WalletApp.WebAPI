namespace WalletApp.WebAPI.Middleware
{
    public class AppUserMiddleware
    {
        private readonly RequestDelegate _next;

        public AppUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var claim = context.User.FindFirst("AppUserId");
                if (claim != null && int.TryParse(claim.Value, out int userId))
                {
                    context.Items["AppUserId"] = userId;
                }
            }

            await _next(context);
        }
    }

}
