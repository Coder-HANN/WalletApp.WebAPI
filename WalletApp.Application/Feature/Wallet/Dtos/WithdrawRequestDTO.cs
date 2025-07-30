using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Enums;


namespace WalletApp.Application.Feature.Wallet.Dtos    
{
    public record WithdrawRequestDTO : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
        public Guid WalletId { get; set; }
        public Guid AppBankAccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        
    }
}
// cüzdan seç, kart seç tutar , açıklama 