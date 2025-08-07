using FluentValidation;
using WalletApp.Application.Feature.Wallet.Commands;

namespace WalletApp.Application.Feature.Wallet.Validations;

public class DepositCommandValidator : AbstractValidator<DepositCommand>
{
    public DepositCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalı.");
    }
}
