using MediatR;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Feature.Queries;


namespace WalletApp.Application.Feature.Handler
{
    public class GetUserWalletsQueryHandler : IRequestHandler<GetUserWalletsQuery, ServiceResponse<IEnumerable<CreateWalletResponseDTO>>>
    {
        private readonly WalletService _walletService;

        public GetUserWalletsQueryHandler(WalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<ServiceResponse<IEnumerable<CreateWalletResponseDTO>>> Handle(GetUserWalletsQuery request, CancellationToken cancellationToken)
        {
            var wallets = await _walletService.GetWalletsByUserIdAsync(request.UserId);

            var dtoList = wallets.Select(w => new CreateWalletResponseDTO
            {
                Id = w.Id,
                UserId = w.UserId,
                TotalBalance = w.TotalBalance,
                Assest = w.Assest,
                Currency = w.Currency
            });

            return ServiceResponse<IEnumerable<CreateWalletResponseDTO>>.Ok(dtoList, "Cüzdanlar getirildi.");
        }
    }
}
