namespace WalletApp.Application.DTOs.Auth
{
    public class LoginUserResponseDTO
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public int AppUserId { get; set; }
        public DateTime TokenExpiration { get; set; }
        
    }
}
