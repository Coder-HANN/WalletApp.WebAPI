using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Auth.Dtos
{
    public class VerifyEmailRequestDTO : IRequest<ServiceResponse<string>>
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
