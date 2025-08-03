using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.User.Dtos.UserProfile;
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
        public async Task<ServiceResponse<UserProfileResponseDTO>> UpdateProfile([FromBody] UserProfileRequestDTO command)
        {
            return await _mediator.Send(command);
        }

    }
}
