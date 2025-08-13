using FluentValidation;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.BankAccount.Validations;

public class BankTransferValidation : AbstractValidator<BankTransferCommand>
{
    public BankTransferValidation()
    {
        RuleFor(x => x.WalletId)
            .NotEmpty().WithMessage("Cüzdan bilgisi zorunludur.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Gönderilecek tutar 0'dan büyük olmalıdır.");

        RuleFor(x => x.RegisterBank)
            .IsInEnum().WithMessage("Geçerli bir banka tipi seçilmelidir.");

        When(x => x.RegisterBank == RegisterBank.Registered, () =>
        {
            RuleFor(x => x.TargetBankAccountId)
                .NotNull().WithMessage("Kayıtlı banka hesabı seçilmelidir.");
        });

        When(x => x.RegisterBank == RegisterBank.External, () =>
        {
            RuleFor(x => x.Iban)
                .NotEmpty().WithMessage("IBAN alanı zorunludur.")
                .MinimumLength(26).WithMessage("IBAN en az 26 karakter olmalıdır.")
                .Must(i => i.Replace(" ", "").StartsWith("TR"))
                .WithMessage("IBAN 'TR' ile başlamalıdır.");
        });
    }
}
