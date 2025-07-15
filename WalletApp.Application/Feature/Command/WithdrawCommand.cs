using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Command
{
    public record WithdrawCommand(Guid WalletId, decimal Amount, string Description, int RequestedBy) : IRequest<TransactionRequestDTO>;

}