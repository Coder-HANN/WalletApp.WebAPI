using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Dtos
{
    public class BankDepositRequestDTO : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
        
        public Guid TargetBankId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
