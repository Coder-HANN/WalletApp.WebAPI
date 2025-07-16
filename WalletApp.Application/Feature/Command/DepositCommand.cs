using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Command
{
    public record DepositCommand(Guid WalletId, int UserID, decimal Amount, string? Description, int RequestedBy) : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
        public int UserId { get; set; }
    }
}