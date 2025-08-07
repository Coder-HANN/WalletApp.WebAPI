using FluentValidation;
using WalletApp.Application.Feature.Wallet.Commands;

namespace WalletApp.Application.Feature.Wallet.Validations;
public class WithdrawCommandValidator : AbstractValidator<WithdrawCommand>
{
    public WithdrawCommandValidator()
    {
        
        RuleFor(x => x.AppBankAccountId).NotEmpty().WithMessage("Banka hesabı ID'si boş olamaz.");
        RuleFor(x => x.WalletId).NotEmpty().WithMessage("Cüzdan ID'si boş olamaz.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalı.");
    }
}
