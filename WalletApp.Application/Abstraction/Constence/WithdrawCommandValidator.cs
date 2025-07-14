using FluentValidation;
using WalletApp.Application.Command;

namespace WalletApp.Application.Abstraction.Constence;

public class WithdrawCommandValidator : AbstractValidator<WithdrawCommand>
{
    public WithdrawCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalı.");
        RuleFor(x => x.RequestedBy).GreaterThan(0);
    }
}
