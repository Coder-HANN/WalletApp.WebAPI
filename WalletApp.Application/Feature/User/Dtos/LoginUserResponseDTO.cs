namespace WalletApp.Application.Feature.User.Dtos
{
    public class LoginUserResponseDTO
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public int AppUserId { get; set; }
        public DateTime TokenExpiration { get; set; }
        
    }
}
