using FluentValidation;
using WalletApp.Application.Feature.User.Commands;

namespace WalletApp.Application.Feature.User.Validators
{
    public class DeleteUserAccountValidation : AbstractValidator<DeleteUserAccountCommand>
    {
        public DeleteUserAccountValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.Command).NotEmpty().WithMessage("Command is required.")
                .MaximumLength(250).WithMessage("Command must not exceed 250 characters.");
        }
    }
}
