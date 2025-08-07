using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.DTOs.ProfileUpdate;
using WalletApp.Application.Feature.ProfileUpdate.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;
namespace WalletApp.WebAPI.Controllers.Public
{
    [ApiController]
    [Route("api/public/[controller]")]
    [ApiExplorerSettings(GroupName = "Public")]

    public class UsersContreller : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersContreller(IMediator mediator) => _mediator = mediator;

        [HttpPut("ProfileUpdate")]
        public async Task<ServiceResponse<UserProfileResponseDTO>> UpdateProfile([FromBody] UserProfileUpdateCommand command)
        {
            return await _mediator.Send(command);
        }

    }
}
