using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;

[ApiController]
[Route("api/public/[controller]")]
[ApiExplorerSettings(GroupName = "Public")]
public class BankAccountController : ControllerBase
{
    private readonly IMediator _mediator;
    public BankAccountController(IMediator mediator) => _mediator = mediator;


    [HttpPost("Add")]
    public async Task<ServiceResponse<BankAccountCommand>> AddBankAccount([FromBody] BankAccountCommand command)
    {
        return await _mediator.Send(command);
    }
    [HttpDelete("Delete")]
    public async Task<ServiceResponse<string>> DeleteBankAccount([FromBody] DeleteBankAccountCommand command)
    {
        return await _mediator.Send(command);
    }
}


