using MediatR;
using WalletApp.Application.DTO;

namespace WalletApp.Application.Command;

public record WithdrawCommand(
    Guid WalletId,
    decimal Amount,
    string Description,
    int RequestedBy
) : IRequest<TransactionRequestDTO>;
