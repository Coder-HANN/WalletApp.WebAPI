using FluentValidation;
using WalletApp.Application.Feature.Command;

namespace WalletApp.Application.Feature.Constence;

public class WithdrawCommandValidator : AbstractValidator<WithdrawCommand>
{
    public WithdrawCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalı.");
        RuleFor(x => x.RequestedBy).GreaterThan(0);
    }
}
