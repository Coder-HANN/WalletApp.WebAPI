using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Wallet.Handlers
{
    public class GetWalletHistoryQueryHandler : IRequestHandler<GetUserWalletsHistoryQueryRequestDTO, ServiceResponse<IEnumerable<TransactionResponseDTO>>>
    {
        private readonly WalletService _walletService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetWalletHistoryQueryHandler(WalletService walletService, IHttpContextAccessor httpContextAccessor)
        {
            _walletService = walletService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<IEnumerable<TransactionResponseDTO>>> Handle(GetUserWalletsHistoryQueryRequestDTO request, CancellationToken cancellationToken)
        {
            var transactions = await _walletService.GetTransactionHistoryAsync(request.WalletId);

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
