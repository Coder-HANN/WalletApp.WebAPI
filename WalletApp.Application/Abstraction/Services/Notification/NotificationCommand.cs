using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Abstraction.Services.Notification
{
    public class NotificationCommand
    {
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
