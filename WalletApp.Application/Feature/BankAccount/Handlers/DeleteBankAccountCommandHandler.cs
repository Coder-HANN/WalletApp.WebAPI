using MediatR;
using WalletApp.Application.Feature.BankAccount.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;

public class DeleteBankAccountCommandHandler : IRequestHandler<DeleteBankAccountRequestDTO, ServiceResponse<string>>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ICurrentUserService _currentUser;

    public DeleteBankAccountCommandHandler(
        IBankAccountRepository bankAccountRepository,
        ICurrentUserService currentUser)
    {
        _bankAccountRepository = bankAccountRepository;
        _currentUser = currentUser;
    }

    public async Task<ServiceResponse<string>> Handle(DeleteBankAccountRequestDTO request, CancellationToken cancellationToken)
    {
        var account = await _bankAccountRepository.GetAsync(x => x.Iban == request.Iban);

        if (account == null)
            return ServiceResponse<string>.Fail("Banka hesabı bulunamadı.");

        if (account.AppUserId != _currentUser.CurrentUser())
            return ServiceResponse<string>.Fail("Bu hesabı silme yetkiniz yok.");

        await _bankAccountRepository.DeleteAsync(account);

        return ServiceResponse<string>.Ok("Banka hesabı başarıyla silindi.");
    }
}
