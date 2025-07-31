using MediatR;

namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public record DepositRequestDTO : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
      public Guid WalletId { get; set; }
        public Guid SourceBankId { get; set; } 
        public int AppUserId { get; set; }
        public decimal Amount { get; set; }
        public string? Description {  get; set; }
        
       
    }
}

