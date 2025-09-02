using Microsoft.AspNetCore.SignalR;

namespace WalletApp.Application.Abstraction.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubServices;

        public NotificationService(IHubContext<NotificationHub> hubServices)
        {
            _hubServices = hubServices;
        }

        public async Task SendToUserAsync(string userId,string message)
        {
            await _hubServices.Clients.User(userId).SendAsync(message);
        }

        public async Task SendToAllAsync(string message)
        {
            await _hubServices.Clients.All.SendAsync("", message);
        }
    }
}
