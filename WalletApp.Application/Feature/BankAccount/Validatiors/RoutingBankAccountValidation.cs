using FluentValidation;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Application.Feature.BankAccount.Validatiors
{
    public class RoutingBankAccountValidation : AbstractValidator<RoutingBankAccountCommand>
    {
        public RoutingBankAccountValidation()
        {
            RuleFor(x => x.SourceProviderBankId == Guid.Empty)
                .Equal(false).WithMessage("SourceProviderBankId is required.");
        }
    }
}
