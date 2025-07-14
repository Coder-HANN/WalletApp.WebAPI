using FluentValidation;

public class CreateBankAccountRequestValidator : AbstractValidator<CreateBankAccountRequestDTO>
{
    public CreateBankAccountRequestValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.Bilgiler).NotEmpty().MaximumLength(500);
    }
}