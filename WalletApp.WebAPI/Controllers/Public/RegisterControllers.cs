using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.DTOs.Auth;
using WalletApp.Application.Feature.Auth.Commands;
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
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ServiceResponse<RegisterResponseDTO>> Register([FromBody] RegisterCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
