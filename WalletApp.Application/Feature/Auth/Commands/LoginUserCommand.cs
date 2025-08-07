using MediatR;
using WalletApp.Application.DTOs.Auth;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Auth.Commands
{
    public class LoginUserCommand : IRequest<ServiceResponse<LoginUserResponseDTO>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
