using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BankController : ControllerBase
{
    private readonly IMediator _mediator;
    public BankController(IMediator mediator) => _mediator = mediator;

    [HttpPost("transfer")]
    public async Task<ServiceResponse<TransactionResponseDTO>> BankTransfer([FromBody] BankTransferRequestDTO command)
    {
        return await _mediator.Send(command);
    }
}