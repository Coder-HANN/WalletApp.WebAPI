using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Handlers;


namespace WalletApp.Application.Feature.Wallet.Queries
{
    public class GetWalletHistoryQueryCommandHandler : IRequestHandler<GetUserWalletsHistoryQuery, ServiceResponse<IEnumerable<TransactionResponseDTO>>>
    {
        private readonly WalletService _walletService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

        public GetWalletHistoryQueryCommandHandler(
            WalletService walletService,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService)
        {
            _walletService = walletService;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<IEnumerable<TransactionResponseDTO>>> Handle(GetUserWalletsHistoryQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _walletService.GetWalletTransactionHistoryAsync(request.WalletId);

            if (!transactions.Any())
                return ServiceResponse<IEnumerable<TransactionResponseDTO>>.Fail("İşlem geçmişi bulunamadı.");

            var dtoList = transactions.Select(t => new TransactionResponseDTO
            {
                Id = t.Id,
                WalletId = t.WalletId,
                Amount = t.Amount,
                Type = t.Type,
                Description = t.Description,
                CreatedDate = t.CreatedDate
            });

            return ServiceResponse<IEnumerable<TransactionResponseDTO>>.Ok(dtoList, "İşlem geçmişi getirildi.");
        }
    }
}