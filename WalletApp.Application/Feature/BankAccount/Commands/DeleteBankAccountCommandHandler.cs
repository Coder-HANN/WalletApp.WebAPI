using MediatR;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.BankAccount.Validatiors.Resource;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

public class DeleteBankAccountCommandHandler : IRequestHandler<DeleteBankAccountCommand, ServiceResponse<string>>
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

    public async Task<ServiceResponse<string>> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<string>.Fail(DeleteBankResource.UserIsNotFound);

        var account = await _bankAccountRepository.GetAsync(x => x.Iban == request.Iban);

        if (account == null)
            return ServiceResponse<string>.Fail(DeleteBankResource.BankAccountIsNotFound);

        await _bankAccountRepository.DeleteAsync(account);

        return ServiceResponse<string>.Ok(DeleteBankResource.BankAccountDeleted);
    }
}
