
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.Auth.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class DeleteUserAccountServices : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeleteUserAccountServices(IMediator mediator)
        {
            _mediator = mediator;
        }
    
        [HttpDelete("DeleteUserAccount")]
        public async Task<ServiceResponse<string>> DeleteUserAccount([FromBody] DeleteUserAccountRequestDTO command)
        {
            return await _mediator.Send(command);

        }
    } 
}