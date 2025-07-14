using MediatR;
using WalletApp.Application.DTO;

namespace WalletApp.Application.Command
{
    public class LoginUserCommand : IRequest<LoginResponseDTO>
    {

        public LoginUserCommand() { }
        public UserRequestDTO RequestDTO { get; set; }

        public LoginUserCommand(LoginUserCommand dto) { }

        public LoginUserCommand(UserRequestDTO requestDTO)
        {
            RequestDTO = requestDTO;
        }
    }
}
