using MediatR;


namespace WalletApp.Application.DTO
{
    public class BankTransferCommand : IRequest<Transection>
    {
        public BankTransferRequestDTO BankTransferRequest { get; }

        public BankTransferCommand(BankTransferRequestDTO request)
        {
            BankTransferRequest = request;
        }
    }
}
