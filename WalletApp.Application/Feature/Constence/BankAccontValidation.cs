using FluentValidation;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Constence;
public class CreateBankAccountRequestValidator : AbstractValidator<CreateBankAccountRequestDTO>
{
    public CreateBankAccountRequestValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.Bilgiler).NotEmpty().MaximumLength(500);
    }
}