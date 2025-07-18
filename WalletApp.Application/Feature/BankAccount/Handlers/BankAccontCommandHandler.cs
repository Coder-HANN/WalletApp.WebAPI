using MediatR;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Application.Feature.Wallet.Dtos;
using Microsoft.AspNetCore.Http;
using WalletApp.Domain.Entities;


namespace WalletApp.Application.Feature.BankAccount.Handlers
{
    public class BankAccountCommandHandler : IRequestHandler<BankAccountRequestDTO, ServiceResponse<BankAccountRequestDTO>>
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public BankAccountCommandHandler(IBankAccountRepository bankAccountRepository, IHttpContextAccessor httpContextAccessor)
        {
            _bankAccountRepository = bankAccountRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<BankAccountRequestDTO>> Handle(BankAccountRequestDTO request, CancellationToken cancellationToken)
        {
            var entity = new AppBankAccount
            {
                AppUserId = (int)request.AppUserId, // dikkat: cast gerekiyor çünkü int
                WalletId = request.WalletId,
                AccountName = request.AccountName,
                Iban = request.Iban,
                BankName = request.BankName,
                AccountType = request.AccountType,
                Balance = request.Balance,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _bankAccountRepository.AddAsync(entity);

            var dto = new BankAccountRequestDTO
            {
                AccountName = entity.AccountName,
                Iban = entity.Iban,
                BankName = entity.BankName,
                AccountType = entity.AccountType,
                Balance = entity.Balance,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            return ServiceResponse<BankAccountRequestDTO>.Ok(dto, "Banka hesabı başarıyla eklendi.");
        }
    }
}
