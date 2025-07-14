using MediatR;
using WalletApp.Application.DTO;
namespace WalletApp.Application.Features.Wallet.Commands.Deposit;

public record DepositCommand(
    Guid WalletId,
    decimal Amount,
    string? Description,
    int RequestedBy     // Controller'da token'dan ekleniyor
) : IRequest<TransactionRequestDTO>;
