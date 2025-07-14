using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Command.BankAccountCommand;
using WalletApp.Application.DTO;

namespace WalletApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int GetUserId() => int.Parse(User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException());

    [HttpPost("add")]
    public async Task<IActionResult> AddBankAccount([FromBody] CreateBankAccountRequest request)
    {
        var command = new CreateBankAccountCommand(GetUserId(), request.WalletId, request.Bilgiler);
        var result = await _mediator.Send(command);
        return HandleResponse(result);
    }

    private IActionResult HandleResponse<T>(ServiceResponse<T> response)
    {
        if (response.Success) return Ok(response);
        if (response.Message?.ToLower().Contains("yetki") == true) return Forbid();
        return BadRequest(response);
    }
}

public class CreateBankAccountRequest
{
    public Guid WalletId { get; set; }
    public string Bilgiler { get; set; }
}
