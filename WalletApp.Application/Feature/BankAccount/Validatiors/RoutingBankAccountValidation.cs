using FluentValidation;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Application.Feature.BankAccount.Validatiors
{
    public class RoutingBankAccountValidation : AbstractValidator<RoutingBankAccountCommand>
    {
        public RoutingBankAccountValidation()
        {
            RuleFor(x => x.Iban).MaximumLength(34)
                .WithMessage("IBAN must be at most 34 characters long.")
                .NotEmpty().WithMessage("IBAN is required.")
                .Matches(@"^[A-Z]{2}[0-9]{2}[A-Z0-9]{1,30}$")
                .WithMessage("IBAN must start with two uppercase letters, followed by two digits, and then up to 30 alphanumeric characters.");
        }
    }
}
