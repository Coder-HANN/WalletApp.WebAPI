using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Command
{
    public class LoginUserCommand : IRequest<ServiceResponse<LoginResponseDTO>>
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
