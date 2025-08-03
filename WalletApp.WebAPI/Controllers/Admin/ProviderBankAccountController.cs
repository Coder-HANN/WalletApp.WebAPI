using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.BankAccount.Dtos;

namespace WalletApp.WebAPI.Controllers.Admin
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [Authorize(Roles = "Admin")]
    public class ProviderBankAccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProviderBankAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddProviderBankAccount")]
        public async Task<IActionResult> AddProviderBankAccount([FromBody] ProviderBankAccountRequestDTO command)
        {
            
            return (IActionResult)await _mediator.Send(command);
            
        }
    }
}
