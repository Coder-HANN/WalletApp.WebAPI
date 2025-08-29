using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Application.Feature.Wallet.Validations.Resource;

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
                return ServiceResponse<IEnumerable<AppWalletResponseDTO>>.Fail(GetUserWalletsQueryResource.UserIsNotFound);

            var wallets = await _walletService.GetMyWalletsAsync(currentUserId);


            var dtoList = wallets.Select(w => new AppWalletResponseDTO
            {
                Id = w.Id,
                AppUserId = currentUserId,
                TotalBalance = w.TotalBalance,
                Assest = w.Assest,
                
            });

            return ServiceResponse<IEnumerable<AppWalletResponseDTO>>.Ok(dtoList, GetUserWalletsQueryResource.SuccessMessage);
        }
    }
}
