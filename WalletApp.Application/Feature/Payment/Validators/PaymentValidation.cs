using FluentValidation;
using WalletApp.Application.DTOs.Payment;

namespace WalletApp.Application.Feature.Payment.Validators
{
    public class PaymentValidation : AbstractValidator<PaymentCommand>
    {
        public PaymentValidation()
        {
            RuleFor(x => x.AppWalletId).NotEmpty().WithMessage("App Wallet ID is required.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
            
        }
    }
}
