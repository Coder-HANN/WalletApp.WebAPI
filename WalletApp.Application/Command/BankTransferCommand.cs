using MediatR;
using WalletApp.Application.DTO;
using WalletApp.Domain.Base;

namespace WalletApp.Application.Command
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
