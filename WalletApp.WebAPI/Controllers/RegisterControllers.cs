using Microsoft.AspNetCore.Mvc;
using MediatR;
using WalletApp.Application.Feature.User.Dtos;
namespace WalletApp.WebAPI.Controllers
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
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
