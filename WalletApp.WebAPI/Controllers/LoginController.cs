using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Login([FromBody] LoginUserCommand dto)
        {
            var command = new LoginUserCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(result); // 200 OK
        }
    }
}

