using MediatR;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Services.Repositories.EntitysRepository;
using WalletApp.Domain.Base;

namespace WalletApp.Application.Feature.Handler;
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