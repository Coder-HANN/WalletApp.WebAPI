using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Queries
{
    public record GetUserWalletsQuery(int UserId) : IRequest<IEnumerable<CreateWalletDTO>>;
}
