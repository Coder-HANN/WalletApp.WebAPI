using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Queries;


[ApiController]
[Route("api/public/[controller]")]
[ApiExplorerSettings(GroupName = "Public")]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;
    public WalletController(IMediator mediator) => _mediator = mediator;


    [HttpPost("Create")]
    public async Task<ServiceResponse<AppWalletResponseDTO>> CreateWallet([FromBody] AppWalletCommand command)
    {
        return await _mediator.Send(command);
         
    }

    [HttpPost("Deposit")]
    public async Task<ServiceResponse<TransactionResponseDTO>> Deposit([FromBody] DepositCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("Withdraw")]
    public async Task<ServiceResponse<TransactionResponseDTO>> Withdraw([FromBody] WithdrawCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("Transfer")]
    public async Task<ServiceResponse<TransactionResponseDTO>> Transfer([FromBody] TransferCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("All")]
    public async Task<ServiceResponse<IEnumerable<AppWalletResponseDTO>>> GetUserWallets([FromBody] GetUserWalletsQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpPost("{walletId:guid}/History")]
    public async Task<ServiceResponse<IEnumerable<TransactionResponseDTO>>> GetHistory([FromQuery] GetUserWalletsHistoryQuery query)
    {
        return await _mediator.Send(query);
    }
}

