using MediatR;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.Wallet.Queries
{
    public class GetUserWalletsHistoryQuery : IRequest<ServiceResponse<IEnumerable<TransactionResponseDTO>>>
    {
        public Guid WalletId { get; set; }
        
        public GetUserWalletsHistoryQuery(Guid walletId)
        {
            WalletId = walletId;
            
        }
        public GetUserWalletsHistoryQuery() { }
    }
        

}

