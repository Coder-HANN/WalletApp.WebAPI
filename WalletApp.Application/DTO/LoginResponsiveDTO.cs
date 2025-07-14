

namespace WalletApp.Application.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public DateTime TokenExpiration { get; set; }
        // Diğer gerekli kullanıcı bilgilerini ekleyebilirsiniz
    }
}
