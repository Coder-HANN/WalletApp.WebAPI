using MediatR;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.Wallet.Commands
{
    public record DepositCommand : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
      public Guid WalletId { get; set; }
        public Guid SourceBankId { get; set; } 
       
        public decimal Amount { get; set; }
        public DescriptionType Description {  get; set; }
        
       
    }
}

