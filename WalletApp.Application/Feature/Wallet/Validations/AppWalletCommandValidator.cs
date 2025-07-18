using FluentValidation;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Wallet.Validations;
public class AppWalletCommandValidator : AbstractValidator<AppWalletRequestDTO>
{
    public AppWalletCommandValidator()
    {
        RuleFor(x => x.AppUserId).GreaterThan(0);
        RuleFor(x => x.Asset).NotEmpty().MaximumLength(10);
    }
}