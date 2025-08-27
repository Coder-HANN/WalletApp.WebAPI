using Braintree;
using MediatR;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.Wallet.Queries
{
    public class GetUserWalletsHistoryQuery : IRequest<ServiceResponse<IEnumerable<TransactionResponseDTO>>>
    {
        public Guid WalletId { get; set; }
        public int Page { get; set; } 
        public int PageSize { get; set; } 

        public GetUserWalletsHistoryQuery(Guid walletId, int Page, int PageSize)
        {
            WalletId = walletId;
            this.Page = Page;
            this.PageSize = PageSize;
        }
        public GetUserWalletsHistoryQuery() { }
    }
}

