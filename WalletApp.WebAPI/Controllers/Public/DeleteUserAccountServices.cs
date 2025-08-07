
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.User.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.WebAPI.Controllers.Public
{
    [ApiController]
    [Route("api/public/[controller]")]
    [ApiExplorerSettings(GroupName = "Public")]
    public class DeleteUserAccountServices : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeleteUserAccountServices(IMediator mediator)
        {
            _mediator = mediator;
        }
    
        [HttpDelete("DeleteUserAccount")]
        public async Task<ServiceResponse<string>> DeleteUserAccount([FromBody] DeleteUserAccountCommand command)
        {
            return await _mediator.Send(command);

        }
    } 
}