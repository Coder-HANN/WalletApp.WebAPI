using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Abstraction.Services.Notification
{
    public interface INotificationService
    {
        Task SendToUser(string userId,string message);
        Task SendToAll(string message);
    }
}
