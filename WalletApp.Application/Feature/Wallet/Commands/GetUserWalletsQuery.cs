using MediatR;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;

public class GetUserWalletsQuery: IRequest<ServiceResponse<IEnumerable<AppWalletResponseDTO>>>
{
    
    public Guid WalletId { get; set; }
  

    public GetUserWalletsQuery() { }

    public GetUserWalletsQuery(Guid walletId)
    {
        WalletId = walletId;
        
    }
}
