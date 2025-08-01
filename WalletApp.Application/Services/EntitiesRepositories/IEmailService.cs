using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Services.EntitiesRepositories
{
    public interface IEmailService
    {
        void Remove(string cacheKey);
        Task SendAsync(string to, string subject, string body);
        bool TryGetValue(string cacheKey, out string? storedCode);
    }
}
