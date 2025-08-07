using MediatR;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class BankDepositCommand : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
        
        public Guid TargetBankId { get; set; }
        public decimal Amount { get; set; }
        public DescriptionType Description { get; set; }
    }
}
