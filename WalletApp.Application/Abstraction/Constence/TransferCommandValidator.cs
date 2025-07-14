using FluentValidation;
using WalletApp.Application.Command;

namespace WalletApp.Application.Abstraction.Constence;

public class TransferCommandValidator : AbstractValidator<TransferCommand>
{
    public TransferCommandValidator()
    {
        RuleFor(x => x.SourceWalletId).NotEmpty();
        RuleFor(x => x.TargetWalletId).NotEmpty().NotEqual(x => x.SourceWalletId)
            .WithMessage("Hedef cüzdan, kaynak cüzdandan farklı olmalı.");

        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.RequestedBy).GreaterThan(0);
    }
}
