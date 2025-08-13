using FluentValidation;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Application.Feature.BankAccount.Validations;
public class BankAccountValidation : AbstractValidator<BankAccountCommand>
{
    public BankAccountValidation()
    {
        RuleFor(x => x.AccountName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Lütfen banka hesap adını giriniz. En fazla 100 karakter olmalıdır.");

        RuleFor(x => x.WalletId).NotEmpty().WithMessage("Lütfen geçerli cüzdan seçiniz");

        RuleFor(x => x.Iban).NotEmpty().Matches(@"^[A-Z]{2}\d{2}[A-Z0-9]{4}\d{7,10}$")
            .WithMessage("Lütfen geçerli bir IBAN giriniz. Örnek: TR330006100519786457841326");

        RuleFor(x => x.BankName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Lütfen banka adını giriniz. En fazla 100 karakter olmalıdır.");

        RuleFor(x => x.AccountType)
            .IsInEnum()
            .WithMessage("Lütfen geçerli bir hesap türü seçiniz.");

    }
}