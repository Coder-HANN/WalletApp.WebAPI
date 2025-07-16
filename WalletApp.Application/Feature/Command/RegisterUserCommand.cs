using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Command
{
    public class RegisterUserCommand : IRequest<ServiceResponse<RegisterResponseDTO>>
    {
        public RegisterRequestDTO RegisterDTO { get; set; }
        public RegisterUserCommand(RegisterRequestDTO registerDTO)
        {
            RegisterDTO = registerDTO;
        }
    }
}
