using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.User.Dtos.UserProfile
{
    public class UserProfileResponseDTO
    {
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }

    }
}
