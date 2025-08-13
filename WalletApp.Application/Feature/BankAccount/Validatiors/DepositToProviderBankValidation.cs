using FluentValidation;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Application.Feature.BankAccount.Validatiors
{
    public class DepositToProviderBankValidation : AbstractValidator<DepositToProviderBankAccountCommand>
    {
        public DepositToProviderBankValidation()
        {
            RuleFor(x => x.Iban)
                .NotEmpty().WithMessage("IBAN cannot be empty.")
                .Matches(@"^[A-Z]{2}\d{2}[A-Z0-9]{1,30}$").WithMessage("IBAN format is invalid.");
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount cannot be empty.");
           
        }
    }
}
