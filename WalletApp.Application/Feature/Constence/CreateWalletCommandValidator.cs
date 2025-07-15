using FluentValidation;
using WalletApp.Application.Feature.Command;

public class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Assest).NotEmpty().MaximumLength(10);
    }
}
