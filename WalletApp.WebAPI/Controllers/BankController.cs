using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Domain.Base;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BankController : ControllerBase
{
    private readonly IMediator _mediator;
    public BankController(IMediator mediator) => _mediator = mediator;

    private int GetUserId() =>
        int.Parse(User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException());

    [HttpPost("transfer")]
    public async Task<ServiceResponse<TransactionResponseDTO>> BankTransfer([FromBody] BankTransferCommand command)
    {
        command.UserId = GetUserId();
        return await _mediator.Send(command);
    }
}
