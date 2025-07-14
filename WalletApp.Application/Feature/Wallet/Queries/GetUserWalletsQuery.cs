using MediatR;
using WalletApp.Application.DTO;

namespace WalletApp.Application.Feature.Wallet.Query
{
    public record GetUserWalletsQuery(int UserId) : IRequest<IEnumerable<CreateWalletDTO>>;
}
