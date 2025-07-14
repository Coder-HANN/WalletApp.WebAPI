using MediatR;
using WalletApp.Application.DTO;

namespace WalletApp.Application.Command.CreateWalletCommand;
public record CreateWalletCommand(string Name, string Currency, int UserId, string Assest) : IRequest<ServiceResponse<CreateWalletDTO>>;


