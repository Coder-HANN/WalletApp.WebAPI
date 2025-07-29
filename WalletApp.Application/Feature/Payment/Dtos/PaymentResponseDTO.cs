
namespace WalletApp.Application.Feature.Payment.DTO
{
    public class PaymentResponseDTO
    {
        public Guid AppPaymentId { get; set; }
        public string Institution { get; set; }
        public string Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
