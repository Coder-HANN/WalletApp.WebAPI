using MediatR;

namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public class AppWalletResponseDTO : IRequest<AppWalletResponseDTO>
	{
        public Guid Id { get; set; }
        public int AppUserId { get; set; }
        public decimal TotalBalance { get; set; }
        public string Assest { get; set; }
        public object Currency { get; internal set; }
    }
}
