using FluentValidation;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Application.Feature.BankAccount.Validatiors
{
    public class ProviderBankAccountValidation : AbstractValidator<ProviderBankAccountCommand>
    {
        public ProviderBankAccountValidation() 
        { 
            RuleFor(x => x.BankName)
                .NotEmpty().WithMessage("Bank name is required.")
                .MaximumLength(100).WithMessage("Bank name must not exceed 100 characters.");
            RuleFor(x => x.Iban).MaximumLength(34)
                .WithMessage("IBAN must not exceed 34 characters.")
                .Matches(@"^[A-Z]{2}[0-9]{2}[A-Z0-9]{1,30}$")
                .WithMessage("IBAN format is invalid.");
        }
    }
}
