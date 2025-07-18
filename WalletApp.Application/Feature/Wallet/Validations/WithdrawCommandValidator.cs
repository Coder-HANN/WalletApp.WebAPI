using FluentValidation;
using WalletApp.Application.Feature.Wallet.Dtos;



namespace WalletApp.Application.Feature.Wallet.Validations;
public class WithdrawCommandValidator : AbstractValidator<WithdrawRequestDTO>
{
    public WithdrawCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalı.");
        RuleFor(x => x.RequestedBy).GreaterThan(0);
    }
}
