using Microsoft.AspNetCore.Http;
using WalletApp.Application.Abstraction.Services;


namespace WalletApp.Application.Common;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int CurrentUser()
    {

        var userIdInt = _httpContextAccessor.HttpContext?.User?.FindFirst("AppUserId")?.Value;

        return int.TryParse(userIdInt, out var userId) ? userId : -1;
    }
}
