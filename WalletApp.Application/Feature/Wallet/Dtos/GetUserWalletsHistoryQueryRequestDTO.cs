using MediatR;


namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public class GetUserWalletsHistoryQueryRequestDTO : IRequest<ServiceResponse<IEnumerable<TransactionResponseDTO>>>
    {
        public Guid WalletId { get; set; }
        public int UserId { get; set; }
        public GetUserWalletsHistoryQueryRequestDTO(Guid walletId, int userId)
        {
            WalletId = walletId;
            UserId = userId;
        }
    }
}
