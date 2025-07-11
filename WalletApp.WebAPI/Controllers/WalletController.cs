using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WalletApp.Application.DTO;
using WalletApp.Application.Services;
using WalletApp.Domain.Base;


namespace WalletApp.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly WalletService _walletService;

        public WalletController(WalletService walletService)
        {
            _walletService = walletService;
        }

        // ✅ Cüzdan oluştur
        [HttpPost("create")]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletDTO dto)
        {
            int userId = GetUserId(); // Şu anlık sabit alıyoruz
            var result = await _walletService.CreateWalletAsync(userId, dto.Assest);
            return Ok(result);
        }

        // ✅ Kullanıcının tüm cüzdanlarını getir
        [HttpPost("all")]
        public async Task<IActionResult> GetUserWallets()
        {
            int userId = GetUserId(); // Aynı şekilde örnek
            var wallets = await _walletService.GetWalletsByUserIdAsync(userId);
            return Ok(wallets);
        }

        // Bu metot JWT token'dan ya da başka bir yerden UserId alacak şekilde değiştirilebilir
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("userId"); // Eğer token'da "userId" varsa

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("UserId bulunamadı.");

            return int.Parse(userIdClaim.Value);
        }
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionRequestDTO dto)
        {
            var transaction = await _walletService.ProcessWalletTransactionAsync(dto.WalletId, dto.Amount, "Deposit", dto.Description);
            return Ok(transaction);
        }
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionRequestDTO dto)
        {
            var transaction = await _walletService.ProcessWalletTransactionAsync(dto.WalletId, dto.Amount, "Withdraw", dto.Description);
            return Ok(transaction);
        }
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequestDTO dto)
        {
            var transaction = await _walletService.TransferAsync(dto.SourceWalletId, dto.TargetWalletId, dto.Amount);
            return Ok(transaction);
        }
        [HttpGet("{walletId}/history")]
        public async Task<IActionResult> GetHistory(int walletId)
        {
            var history = await _walletService.GetTransactionHistoryAsync(walletId);
            return Ok(history);
        }


    }

}