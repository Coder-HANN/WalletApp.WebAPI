using FluentValidation;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Application.Feature.BankAccount.Validations;
public class CreateBankAccountRequestValidator : AbstractValidator<CreateBankAccountRequestDTO>
{
    public CreateBankAccountRequestValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.Bilgiler).NotEmpty().MaximumLength(500);
    }
}