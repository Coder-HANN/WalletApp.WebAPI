using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.DTOs.Payment
{
    public class PaymentCommand : IRequest<ServiceResponse<PaymentResponseDTO>>
    {
        public Guid AppWalletId { get; set; }
        public string Institution { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DescriptionType Description { get; set; }

        public PaymentCommand(Guid appWalletId, string institution, decimal amount, DateTime paymentDate)
        {
            AppWalletId = appWalletId;
            Institution = institution;
            Amount = amount;
            PaymentDate = paymentDate;
        }
        public PaymentCommand()
        {
        }
    }
}
