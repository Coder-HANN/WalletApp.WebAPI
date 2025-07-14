using MediatR;
using WalletApp.Application.DTO;

namespace WalletApp.Application.Command;

public record TransferCommand(
    Guid SourceWalletId,
    Guid TargetWalletId,
    decimal Amount,
    int RequestedBy
) : IRequest<TransactionRequestDTO>;
