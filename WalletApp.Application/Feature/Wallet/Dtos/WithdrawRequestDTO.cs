using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Enums;


namespace WalletApp.Application.Feature.Wallet.Dtos    
{
    public record WithdrawRequestDTO : IRequest<ServiceResponse<IList<TransactionResponseDTO>>>
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public TransactionType Type { get; set; }
    }

}
