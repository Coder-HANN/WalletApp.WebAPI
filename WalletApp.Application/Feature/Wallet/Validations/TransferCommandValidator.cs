using FluentValidation;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.Wallet.Validations;
public class TransferCommandValidator : AbstractValidator<TransferRequestDTO>
{
    public TransferCommandValidator()
    {
        RuleFor(x => x.SourceWalletId).NotEmpty();
        RuleFor(x => x.TargetWalletId).NotEmpty().NotEqual(x => x.SourceWalletId)
            .WithMessage("Hedef cüzdan, kaynak cüzdandan farklı olmalı.");

        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
