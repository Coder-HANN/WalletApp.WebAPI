using MediatR;

namespace WalletApp.Application.DTO
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
