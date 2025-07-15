

namespace WalletApp.Application.Feature.DTO
{
    public class RegisterResponseDTO
    {
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Message { get; set; } = "Registered Successfully";
    }
}
