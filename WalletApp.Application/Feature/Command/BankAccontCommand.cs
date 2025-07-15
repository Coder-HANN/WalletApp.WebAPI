using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Command
{
    public record CreateBankAccountCommand(int UserId, Guid WalletId, string Bilgiler) : IRequest<ServiceResponse<bool>>;
    

}