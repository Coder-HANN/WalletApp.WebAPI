using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Command;
using WalletApp.Application.Command.CreateWalletCommand;
using WalletApp.Application.Feature.Wallet.Query;
using WalletApp.Application.Features.Wallet.Commands.Deposit;
using WalletApp.Application.Features.Wallet.Queries.GetWalletHistory;

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
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletCommand command)
        {
            command = command with { UserId = GetUserId() };
            var result = await _mediator.Send(command);
            return Ok(result);
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
