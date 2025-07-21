using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.User.Dtos.UserProfile
{
    public class UserProfileRequestDTO : IRequest<ServiceResponse<UserProfileResponseDTO>>
    {
        public int AppUserId { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public DateOnly BirthDay { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }

    }
}
