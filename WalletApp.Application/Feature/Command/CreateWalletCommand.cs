using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Command
{
    public record CreateWalletCommand : IRequest<ServiceResponse<CreateWalletResponseDTO>>
    {
        public string Name { get; set; }
        public string Currency { get; set; }
        public int UserId { get; set; }
        public string Assest { get; set; } 
    }

}

