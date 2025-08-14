using FluentValidation;
using WalletApp.Application.Feature.Auth.Commands;

namespace WalletApp.Application.Feature.Auth.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Email is not value.");
            RuleFor(x => x.PhoneNumber)
                .NotNull().WithMessage("Phone number is not null.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");
            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password boş olamaz.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(6).WithMessage("Password must not exceed 6 characters.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name boş olamaz.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname boş olamaz.")
                .MaximumLength(50).WithMessage("Surname must not exceed 50 characters.");
            RuleFor(x => x.BirthDay)
                .NotEmpty().WithMessage("Date of birth boş olamaz.")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");
            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Cinsiyet boş olamaz.");
            RuleFor(x => x.Occupation)
                .NotEmpty().WithMessage("Occupation boş olamaz.");
                
        }

    }
}
