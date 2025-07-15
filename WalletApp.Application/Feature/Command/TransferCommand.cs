using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Command
{
    public record TransferCommand(Guid SourceWalletId, Guid TargetWalletId, decimal Amount, int RequestedBy) : IRequest<TransactionRequestDTO>;
}