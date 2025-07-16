using MediatR;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Services.Repositories.EntitysRepository;
using WalletApp.Domain.Base;


namespace WalletApp.Application.Feature.Handler
{
    public class BankAccountCommandHandler : IRequestHandler<BankAccountCommand, ServiceResponse<BankAccountRequestDTO>>
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public BankAccountCommandHandler(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<ServiceResponse<BankAccountRequestDTO>> Handle(BankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = (int)request.UserId, // dikkat: cast gerekiyor çünkü int
                WalletId = request.WalletId,
                AccountName = request.AccountName,
                Iban = request.Iban,
                BankName = request.BankName,
                BranchName = request.BranchName,
                AccountType = request.AccountType,
                Balance = request.Balance,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _bankAccountRepository.AddAsync(entity);

            var dto = new BankAccountRequestDTO
            {
                Id = entity.Id,
                AccountName = entity.AccountName,
                Iban = entity.Iban,
                BankName = entity.BankName,
                BranchName = entity.BranchName,
                AccountType = entity.AccountType,
                Balance = entity.Balance,
                CreatedAt = entity.CreatedDate,
                UpdatedAt = entity.UpdatedDate
            };

            return ServiceResponse<BankAccountRequestDTO>.Ok(dto, "Banka hesabı başarıyla eklendi.");
        }
    }
}
