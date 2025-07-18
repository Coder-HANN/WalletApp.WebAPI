using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.Wallet.Dtos;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;
    public WalletController(IMediator mediator) => _mediator = mediator;


    [HttpPost("create")]
    public async Task<ServiceResponse<AppWalletResponseDTO>> CreateWallet([FromBody] AppWalletRequestDTO command)
    {
        return await _mediator.Send(command);
         
    }

    [HttpPost("deposit")]
    public async Task<ServiceResponse<TransactionResponseDTO>> Deposit([FromBody] DepositRequestDTO command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("withdraw")]
    public async Task<ServiceResponse<TransactionResponseDTO>> Withdraw([FromBody] WithdrawRequestDTO command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("transfer")]
    public async Task<ServiceResponse<TransactionResponseDTO>> Transfer([FromBody] TransferRequestDTO command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("all")]
    public async Task<ServiceResponse<IEnumerable<AppWalletResponseDTO>>> GetUserWallets([FromBody] GetUserWalletsQueryRequestDTO query)
    {
        return await _mediator.Send(query);
    }

    [HttpGet("{walletId:guid}/history")]
    public async Task<ServiceResponse<IEnumerable<TransactionResponseDTO>>> GetHistory([FromRoute] GetUserWalletsHistoryQueryRequestDTO query)
    {
        return await _mediator.Send(query);
    }
}

