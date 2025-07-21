using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.User.Dtos
{
    public class LoginUserRequestDTO : IRequest<ServiceResponse<LoginUserResponseDTO>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
