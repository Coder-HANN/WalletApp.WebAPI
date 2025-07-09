using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.DTO
{
    public class LoginUserCommand : IRequest<string>
    {
        private LoginUserCommand dto;

        public UserRequestDTO RequestDTO { get; set; }
        public LoginUserCommand(UserRequestDTO requestDTO)
        {
                        RequestDTO = requestDTO;
        }

        public LoginUserCommand(LoginUserCommand dto)
        {
            this.dto = dto;
        }
    }
}
