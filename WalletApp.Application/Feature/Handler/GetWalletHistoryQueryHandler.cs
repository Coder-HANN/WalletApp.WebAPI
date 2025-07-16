using MediatR;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Feature.Queries;


namespace WalletApp.Application.Feature.Handler
{
    public class GetWalletHistoryQueryHandler : IRequestHandler<GetWalletHistoryQuery, ServiceResponse<IEnumerable<TransactionResponseDTO>>>
    {
        private readonly WalletService _walletService;

        public GetWalletHistoryQueryHandler(WalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<ServiceResponse<IEnumerable<TransactionResponseDTO>>> Handle(GetWalletHistoryQuery request, CancellationToken cancellationToken)
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
