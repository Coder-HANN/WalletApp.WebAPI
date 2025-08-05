using Microsoft.AspNetCore.Mvc;
using MediatR;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;

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
        public async Task<ServiceResponse<RegisterResponseDTO>> Register([FromBody] RegisterRequestDTO command)
        {
            return await _mediator.Send(command);
        }
    }
}
