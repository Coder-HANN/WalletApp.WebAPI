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

        public async Task SendToUser(string userId,string message)
        {
            await _hubServices.Clients.User(userId).SendAsync("Hesabınıza para girişi oldu.", message);
        }

        public async Task SendToAll(string message)
        {
            await _hubServices.Clients.All.SendAsync("", message);
        }
    }
}
