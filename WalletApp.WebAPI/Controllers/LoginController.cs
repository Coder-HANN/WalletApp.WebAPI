using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Command;
using WalletApp.Application.DTO;

namespace WalletApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response); // Artık genişletilmiş yanıt dönecek
        }
    }
}

