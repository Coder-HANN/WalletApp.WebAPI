using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Feature.Queries;


    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WalletController(IMediator mediator) => _mediator = mediator;

        private int GetUserId() =>
            int.Parse(User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException());

        [HttpPost("create")]
        public async Task<ServiceResponse<CreateWalletResponseDTO>> CreateWallet([FromBody] CreateWalletCommand command)
        {
            command.UserId = GetUserId();
            return await _mediator.Send(command);
        }

        [HttpPost("deposit")]
        public async Task<ServiceResponse<TransactionResponseDTO>> Deposit([FromBody] DepositCommand command)
        {
            command.UserId = GetUserId();
            return await _mediator.Send(command);
        }

        [HttpPost("withdraw")]
        public async Task<ServiceResponse<TransactionResponseDTO>> Withdraw([FromBody] WithdrawCommand command)
        {
            command.UserId = GetUserId();
            return await _mediator.Send(command);
        }

        [HttpPost("transfer")]
        public async Task<ServiceResponse<TransactionResponseDTO>> Transfer([FromBody] TransferCommand command)
        {
            command.UserId = GetUserId();
            return await _mediator.Send(command);
        }
        [HttpPost("all")]
        public async Task<ServiceResponse<IEnumerable<CreateWalletResponseDTO>>> GetUserWallets([FromBody] GetUserWalletsQuery query)
        {
            query.UserId = GetUserId();
            return await _mediator.Send(query);
        }

        [HttpGet("{walletId:guid}/history")]
        public async Task<ServiceResponse<IEnumerable<TransactionResponseDTO>>> GetHistory([FromRoute] Guid walletId)
        {
            var query = new GetWalletHistoryQuery(walletId, GetUserId());
            return await _mediator.Send(query);
        }
    }

