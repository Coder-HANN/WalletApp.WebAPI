using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.Wallet.Dtos    
{
    public record WithdrawRequestDTO(Guid WalletId, decimal Amount, string Description, int RequestedBy) : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
      
    }
}