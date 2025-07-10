using MediatR;

namespace WalletApp.Application.DTO
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
