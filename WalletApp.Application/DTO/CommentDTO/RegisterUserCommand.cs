using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace WalletApp.Application.DTO.CommentDTO
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
