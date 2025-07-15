using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Feature.Queries;


namespace WalletApp.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WalletController(IMediator mediator) => _mediator = mediator;

        /// <summary>JWT içinden userId alır</summary>
        private int GetUserId()
        {
            var claim = User.FindFirst("userId");
            if (claim == null) throw new UnauthorizedAccessException("UserId bulunamadı.");
            return int.Parse(claim.Value);
        }

        /// ✅ Cüzdan oluştur
        [HttpPost("create")]
        public async Task<ServiceResponse<CreateWalletDTO>> CreateWallet([FromBody] CreateWalletCommand command)
        {
            return await _mediator.Send(command);
        }

        /// ✅ Tüm cüzdanları getir
        [HttpPost("all")]
        public async Task<IActionResult> GetUserWallets()
        {
            var result = await _mediator.Send(new GetUserWalletsQuery(GetUserId()));
            return Ok(result);
        }

        /// ✅ Para yatır
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositCommand command)
        {
            var result = await _mediator.Send(command with { RequestedBy = GetUserId() });
            return Ok(result);
        }

        /// ✅ Para çek
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawCommand command)
        {
            var result = await _mediator.Send(command with { RequestedBy = GetUserId() });
            return Ok(result);
        }

        /// ✅ Transfer
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferCommand command)
        {
            var result = await _mediator.Send(command with { RequestedBy = GetUserId() });
            return Ok(result);
        }

        /// ✅ İşlem geçmişi
        [HttpGet("{walletId:guid}/history")]
        public async Task<IActionResult> GetHistory(Guid walletId)
        {
            var result = await _mediator.Send(new GetWalletHistoryQuery(walletId));
            return Ok(result);
        }
    }
}
