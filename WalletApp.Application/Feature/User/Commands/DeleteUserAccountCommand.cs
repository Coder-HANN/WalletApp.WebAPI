using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.User.Commands
{
    public class DeleteUserAccountCommand : IRequest<ServiceResponse<string>>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Command { get; set; }
    }
}
