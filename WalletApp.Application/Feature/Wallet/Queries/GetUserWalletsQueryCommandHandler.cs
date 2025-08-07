using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Handlers;

namespace WalletApp.Application.Feature.Wallet.Queries
{
    public class GetUserWalletsQueryCommandHandler : IRequestHandler<GetUserWalletsQuery, ServiceResponse<IEnumerable<AppWalletResponseDTO>>>
    {
        private readonly WalletService _walletService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

		public GetUserWalletsQueryCommandHandler(
            WalletService walletService, 
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService)
        {
            _walletService = walletService;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<IEnumerable<AppWalletResponseDTO>>> Handle(GetUserWalletsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.CurrentUser();
            if (currentUserId == null)
                return ServiceResponse<IEnumerable<AppWalletResponseDTO>>.Fail("Kullanıcı doğrulanamadı");

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
