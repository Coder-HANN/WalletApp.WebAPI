using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.BankAccount.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.WebAPI.Controllers.Public
{

    [ApiController]
    [Route("api/public/[controller]")]
    [ApiExplorerSettings(GroupName = "Public")]
    public class BankDepositController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BankDepositController(IMediator mediator) => _mediator = mediator;
    

    [HttpPost("BankDeposit")]
        public async Task<ServiceResponse<TransactionResponseDTO>> BankDeposit([FromBody] BankDepositRequestDTO command)
        {
            return  await _mediator.Send(command);


  
        }

    }
}