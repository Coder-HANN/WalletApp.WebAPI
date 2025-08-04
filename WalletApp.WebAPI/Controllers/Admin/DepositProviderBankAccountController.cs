using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.BankAccount.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class DepositProviderBankAccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DepositProviderBankAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("Deposit")]
        public async Task<ServiceResponse<DepositToProviderBankAccountResponseDTO>> Deposit([FromBody] DepositToProviderBankAccountRequestDTO request)
        {
            return await _mediator.Send(request);


        }
    }
}
