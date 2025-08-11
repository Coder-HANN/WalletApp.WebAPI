using MediatR;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Wallet.Commands
{
    public class AppWalletCommand : IRequest<ServiceResponse<AppWalletResponseDTO>>
    {
     
        public string Name { get; set; }
        public decimal TotalBalance { get; set; }
        public string Asset { get; set; }
        public string Currency { get; set; }

    }
}
