using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;

[ApiController]
[Route("api/public/[controller]")]
[ApiExplorerSettings(GroupName = "Public")]
public class BankController : ControllerBase
{
    private readonly IMediator _mediator;
    public BankController(IMediator mediator) => _mediator = mediator;

    [HttpPost("Transfer")]
    public async Task<ServiceResponse<TransactionResponseDTO>> BankTransfer([FromBody] BankTransferCommand command)
    {
        return await _mediator.Send(command);
    }
}