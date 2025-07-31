using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.BankAccount.Dtos;

namespace WalletApp.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class BankDepositController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BankDepositController(IMediator mediator) => _mediator = mediator;
    

    [HttpPost("BankDeposit")]
        public async Task<IActionResult> BankDeposit([FromBody] BankDepositRequestDTO command)
        {
            return (IActionResult) await _mediator.Send(command);


  
        }

    }
}