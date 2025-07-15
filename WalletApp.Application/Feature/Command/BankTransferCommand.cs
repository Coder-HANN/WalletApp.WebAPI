using MediatR;
using WalletApp.Application.Feature.DTO;
using WalletApp.Domain.Base;

namespace WalletApp.Application.Feature.Command
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
