using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.User.Dtos.UserProfile
{
    public class UserProfileRequestDTO : IRequest<ServiceResponse<UserProfileResponseDTO>>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
    }
}
