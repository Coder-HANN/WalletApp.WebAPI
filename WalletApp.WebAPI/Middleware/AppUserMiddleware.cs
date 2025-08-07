using System.Security.Claims;

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
            // ⛔ Bu yolları kontrol dışı bırak
            var path = context.Request.Path.Value?.ToLower();

            if (path != null && (path.Contains("register") || path.Contains("login")))
            {
                Console.WriteLine($"⏭️ Middleware atlandı: {path}");
                await _next(context);
            }
            else
            {

                Console.WriteLine("🔵 AppUserMiddleware çalıştı.");

                if (context.User.Identity?.IsAuthenticated == true)
                {
                    Console.WriteLine("Claims listesi:");
                    foreach (var c in context.User.Claims)
                    {
                        Console.WriteLine($"Claim Type: {c.Type}, Value: {c.Value}");
                    }

                    var claim = context.User.Claims.FirstOrDefault(c =>
                        c.Type.Equals("AppUserId", StringComparison.OrdinalIgnoreCase));

                    if (claim != null)
                    {
                        context.Items["AppUserId"] = claim.Value;
                        Console.WriteLine($"🟢 AppUserId bulundu: {claim.Value}");
                    }
                    else
                    {
                        Console.WriteLine("🟠 AppUserId claim bulunamadı veya geçersiz.");
                    }
                }
                else
                {
                    Console.WriteLine("🔴 Kullanıcı authenticate değil.");
                }

                await _next(context);
            }
        }
    }
}
