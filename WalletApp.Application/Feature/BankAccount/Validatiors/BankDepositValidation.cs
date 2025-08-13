using FluentValidation;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Application.Feature.BankAccount.Validatiors
{
    public class BankDepositValidation : AbstractValidator<BankDepositCommand>
    {
        public BankDepositValidation()
        {
            RuleFor(x => x.TargetBankId).NotEmpty().WithMessage("Hedef banka seçiniz.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Tutar boş olamaz.")
                .GreaterThan(0).WithMessage("Tutar sıfırdan büyük olmalıdır.");


        }
    }
}
