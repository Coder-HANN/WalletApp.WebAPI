using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin")]

    public class BankRouteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankRouteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("BankRoute")]
        public async Task<ServiceResponse<RoutingBankAccountResponseDTO>> BankRoute([FromBody] RoutingBankAccountCommand request)
        {
            return await _mediator.Send(request);

        }
    }
}
