using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Wallet.Handlers
{
    public class GetUserWalletsQueryCommandHandler : IRequestHandler<GetUserWalletsQueryRequestDTO, ServiceResponse<IEnumerable<AppWalletResponseDTO>>>
    {
        private readonly WalletService _walletService;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public GetUserWalletsQueryCommandHandler(WalletService walletService, IHttpContextAccessor httpContextAccessor)
        {
            _walletService = walletService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<IEnumerable<AppWalletResponseDTO>>> Handle(GetUserWalletsQueryRequestDTO request, CancellationToken cancellationToken)
        {
            var wallets = await _walletService.GetWalletsByAppUserIdAsync(request.UserId);


            var dtoList = wallets.Select(w => new AppWalletResponseDTO
            {
                Id = w.Id,
                AppUserId = w.AppUserId,
                TotalBalance = w.TotalBalance,
                Assest = w.Assest,
                
            });

            return ServiceResponse<IEnumerable<AppWalletResponseDTO>>.Ok(dtoList, "Cüzdanlar getirildi.");
        }
    }
}
