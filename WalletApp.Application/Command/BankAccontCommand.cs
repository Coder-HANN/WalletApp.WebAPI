using MediatR;
using WalletApp.Application.DTO;

namespace WalletApp.Application.Command.BankAccountCommand;
public record CreateBankAccountCommand(int UserId, Guid WalletId, string Bilgiler) : IRequest<ServiceResponse<bool>>;
