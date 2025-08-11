using FluentValidation;
using WalletApp.Application.Feature.Wallet.Commands;

namespace WalletApp.Application.Feature.Wallet.Validations;
public class AppWalletCommandValidator : AbstractValidator<AppWalletCommand>
{
    public AppWalletCommandValidator()
    {
       
        RuleFor(x => x.Asset).NotEmpty().MaximumLength(10);
    }
}