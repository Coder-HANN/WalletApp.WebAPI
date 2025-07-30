using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Payment.DTO
{
    public class PaymentRequestDTO : IRequest<ServiceResponse<PaymentResponseDTO>>
    {
        public Guid AppWalletId { get; set; }
        public string Institution { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Description { get; set; }

        public PaymentRequestDTO(Guid appWalletId, string institution, decimal amount, DateTime paymentDate)
        {
            AppWalletId = appWalletId;
            Institution = institution;
            Amount = amount;
            PaymentDate = paymentDate;
        }
        public PaymentRequestDTO()
        {
        }
    }
}
