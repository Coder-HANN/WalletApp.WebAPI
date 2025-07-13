using MediatR;
using WalletApp.Domain.Base;

namespace WalletApp.Application.DTO
{
    public class BankTransferCommand : IRequest<Transaction>
    {
        public BankTransferRequestDTO BankTransferRequest { get; }

        public BankTransferCommand(BankTransferRequestDTO request)
        {
            BankTransferRequest = request;
        }
    }
}
