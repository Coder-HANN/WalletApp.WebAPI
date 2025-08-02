using MediatR;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Auth.Dtos
{
    public class VerifyEmailRequestDTO : IRequest<ServiceResponse<RegisterResponseDTO>>
    {
        public string Email { get; set; }
    }
}
