using Microsoft.AspNetCore.Mvc;
using MediatR;
using WalletApp.Application.Feature.User.Dtos;

namespace WalletApp.WebAPI.Controllers.Public
{
    [ApiController]
    [Route("api/public/[controller]")]
    [ApiExplorerSettings(GroupName = "Public")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        } 

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
