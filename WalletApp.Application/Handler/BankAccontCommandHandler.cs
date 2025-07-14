using MediatR;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Application.Command.BankAccountCommand;
using WalletApp.Application.DTO;
using WalletApp.Domain.Base;

namespace WalletApp.Application.Command.BankAccountCommand;
public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, ServiceResponse<bool>>
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public CreateBankAccountCommandHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<ServiceResponse<bool>> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = new BankAccount
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            WalletId = request.WalletId,
            Bilgiler = request.Bilgiler
        };

        await _bankAccountRepository.AddAsync(entity);
        return ServiceResponse<bool>.Ok(true, "Banka hesabı başarıyla eklendi.");
    }
}