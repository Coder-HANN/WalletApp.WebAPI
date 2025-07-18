namespace WalletApp.Application.Feature.User.Dtos
{
    public class RegisterResponseDTO
    {
    public int AppUserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Message { get; set; } = "Registered Successfully";
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
    }
}
