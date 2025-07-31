using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class BankTransferRequestDTO : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
        
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string Command { get; set; }
        
    }
}
