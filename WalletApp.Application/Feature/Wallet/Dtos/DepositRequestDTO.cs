using MediatR;

namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public record DepositRequestDTO(Guid WalletId, int AppUserId, decimal Amount, string? Description, int RequestedBy) : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
       
    }
}