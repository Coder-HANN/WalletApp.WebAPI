using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using WalletApp.Application.Feature.User.Dtos.UserProfile;
namespace WalletApp.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class UsersContreller : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersContreller(IMediator mediator) => _mediator = mediator;

        [HttpPut("ProfileUpdate")]

        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileRequestDTO command)
        {
            return (IActionResult)await _mediator.Send(command);
        }
    }
}
