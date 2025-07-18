using MediatR;
using WalletApp.Application.Feature.User.Dtos.UserProfile;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.User.Dtos
{
    public class UserProfileQueryResponseDTO : IRequest<ServiceResponse<UserProfileResponseDTO>>
    {
        public int AppUserId { get; set; }
    }
}
