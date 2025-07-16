using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;
    public LoginController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<ServiceResponse<LoginResponseDTO>> Login([FromBody] LoginUserCommand command)
    {
        return await _mediator.Send(command);
    }
}
