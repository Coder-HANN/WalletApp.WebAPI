using MediatR;
using WalletApp.Application.Feature.DTO;
using WalletApp.Domain.Base;

namespace WalletApp.Application.Feature.Command
{
    public class BankTransferCommand : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
        public BankTransferRequestDTO BankTransferRequest { get; }
        public int UserId { get; set; }

        public BankTransferCommand(BankTransferRequestDTO request)
        {
            BankTransferRequest = request;
        }
    }
}
