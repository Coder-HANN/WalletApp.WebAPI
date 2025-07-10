using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.DTO;
using MediatR;
namespace WalletApp.WebAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        } 
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO dto)
        {
            var command = new RegisterUserCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(result); // 200 OK
        }
    }

}
