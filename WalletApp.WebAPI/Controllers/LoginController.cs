using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;
    public LoginController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<ServiceResponse<LoginUserResponseDTO>> Login([FromBody] LoginUserRequestDTO command)
    {
        return await _mediator.Send(command);
    }
}
