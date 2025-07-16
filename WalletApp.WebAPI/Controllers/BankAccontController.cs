using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator) => _mediator = mediator;

    private int GetUserId() =>
        int.Parse(User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException());

    [HttpPost("add")]
    public async Task<ServiceResponse<BankAccountRequestDTO>> AddBankAccount([FromBody] BankAccountCommand command)
    {
        command.UserId = GetUserId();
        return await _mediator.Send(command);
    }
}

