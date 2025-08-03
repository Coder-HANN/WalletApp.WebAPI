using Microsoft.AspNetCore.Mvc;
using MediatR;
using WalletApp.Application.Feature.Payment.DTO;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.WebAPI.Controllers.Public
{

    [ApiController]
    [Route("api/public/[controller]")]
    [ApiExplorerSettings(GroupName = "Public")]
    public class PaymentControllers : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentControllers(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Payment")]
        public async Task<ServiceResponse<PaymentResponseDTO>> Payment([FromBody] PaymentRequestDTO request)
        {
             return await _mediator.Send(request);
        }
    }
}
