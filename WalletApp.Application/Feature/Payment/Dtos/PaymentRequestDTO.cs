using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Payment.DTO
{
    public class PaymentRequestDTO : IRequest<ServiceResponse<PaymentResponseDTO>>
    {
        public Guid AppWalletId { get; set; }
        public string Institution { get; set; }
        public string Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
    
        
}
