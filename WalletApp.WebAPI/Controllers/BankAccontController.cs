using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IMediator _mediator;
    public BankAccountController(IMediator mediator) => _mediator = mediator;


    [HttpPost("add")]
    public async Task<ServiceResponse<BankAccountRequestDTO>> AddBankAccount([FromBody] BankAccountRequestDTO command)
    {
        return await _mediator.Send(command);
    }
}

