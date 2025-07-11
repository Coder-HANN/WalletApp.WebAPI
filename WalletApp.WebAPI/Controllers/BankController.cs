
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.DTO;

namespace WalletApp.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Kullanıcının cüzdanından seçilen bankaya para transfer eder.
        /// </summary>
        /// <param name="dto">Transfer detayları</param>
        [HttpPost("transfer")]
        public async Task<IActionResult> BankTransfer([FromBody] BankTransferRequestDTO dto)
        {
            int userId = GetUserId();

            // ⚠️ Opsiyonel: userId doğrulaması eklenebilir (örneğin dto içinde varsa)

            var command = new BankTransferCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// JWT token'dan userId çeker. Token'da "userId" claim'inin olması gerekir.
        /// </summary>
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("JWT token'da 'userId' claim bulunamadı.");

            return int.Parse(userIdClaim.Value);
        }
    }
}
