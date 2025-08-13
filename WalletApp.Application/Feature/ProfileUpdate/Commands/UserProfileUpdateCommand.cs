using MediatR;
using WalletApp.Application.DTOs.ProfileUpdate;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;


namespace WalletApp.Application.Feature.ProfileUpdate.Commands
{
    public class UserProfileUpdateCommand : IRequest<ServiceResponse<UserProfileResponseDTO>>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; } // TODO: Change to enum if needed
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; } // TODO: Change to enum if needed

    }
}
