using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Command
{
    public record CreateWalletCommand(string Name, string Currency, int UserId, string Assest) : IRequest<ServiceResponse<CreateWalletDTO>>;

}

