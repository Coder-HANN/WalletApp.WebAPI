using MediatR;

namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public class AppWalletRequestDTO : IRequest<ServiceResponse<AppWalletResponseDTO>>
    {
        public int AppUserId { get; set; }
        public string Name { get; set; }
        public decimal TotalBalance { get; set; }
        public string Asset { get; set; }
        public string Currency { get; set; }

    }
}
