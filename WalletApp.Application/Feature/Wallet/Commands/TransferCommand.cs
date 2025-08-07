using MediatR;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.Wallet.Commands
{
    public class TransferCommand : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
        public Guid SourceWalletId { get; set; }
        public Guid TargetWalletId { get; set; }
        public decimal Amount { get; set; }
        public DescriptionType Description { get; set; }
        public TransactionType Type { get; set; } 
        
    }   
}       