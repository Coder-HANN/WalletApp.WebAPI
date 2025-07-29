using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public class TransferRequestDTO : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
        public Guid SourceWalletId { get; set; }
        public Guid TargetWalletId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public TransactionType Type { get; set; } 
        
    }
}