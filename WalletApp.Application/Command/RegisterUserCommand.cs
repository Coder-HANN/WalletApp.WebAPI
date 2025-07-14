using MediatR;
using WalletApp.Application.DTO;

namespace WalletApp.Application.Command
{
    public class RegisterUserCommand : IRequest<RegisterResponseDTO>
    {
        public RegisterRequestDTO RegisterDTO { get; set; }

        public RegisterUserCommand(RegisterRequestDTO registerDTO)
        {
            RegisterDTO = registerDTO;
        }
    }
}
