namespace WalletApp.Application.DTOs.Payment
{
    public class PaymentResponseDTO
    {
        public Guid AppPaymentId { get; set; }
        public string Institution { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
