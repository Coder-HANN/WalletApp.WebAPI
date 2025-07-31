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
        private readonly IProviderBankRepository _providerBankRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUser;
        public BankAccountCommandHandler(
            IBankAccountRepository bankAccountRepository,
            IProviderBankRepository providerBankRepository,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUser)

        {
            _bankAccountRepository = bankAccountRepository;
            _providerBankRepository = providerBankRepository;
            _httpContextAccessor = httpContextAccessor;
            _currentUser = currentUser;

        }

        public async Task<ServiceResponse<BankAccountRequestDTO>> Handle(BankAccountRequestDTO request, CancellationToken cancellationToken)
        {
            
            var currentUserId = _currentUser.CurrentUser();
            if (currentUserId == null)
                return ServiceResponse<BankAccountRequestDTO>.Fail("Kullanıcı doğrulanamadı.");
            // BankName'e göre ProviderBank var mı kontrol et
            var providerBank = await _providerBankRepository.GetAsync(p => p.BankName == request.BankName);

            if (providerBank == null)
            {
                providerBank = new ProviderBank
                {
                    Id = Guid.NewGuid(),
                    BankName = request.BankName,
                    CreatedDate = DateTime.UtcNow
                };

                await _providerBankRepository.AddAsync(providerBank);
                await _providerBankRepository.SaveChangesAsync();
            }

            var entity = new AppBankAccount
            {
                AppUserId = _currentUser.CurrentUser(),
                WalletId = request.WalletId,
                AccountName = request.AccountName,
                Iban = request.Iban,
                BankName = request.BankName,
                AccountType = request.AccountType,
                Balance = request.Balance,
                ProviderBankId = providerBank.Id, 
                BankCode = request.Iban.Substring(4,5),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _bankAccountRepository.AddAsync(entity);
            await _bankAccountRepository.SaveChangesAsync();

            var dto = new BankAccountRequestDTO
            {
                AccountName = entity.AccountName,
                Iban = entity.Iban,
                BankName = entity.BankName,
                AccountType = entity.AccountType,
                Balance = entity.Balance,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                AppUserId = entity.AppUserId,
                WalletId = entity.WalletId
            };

            return ServiceResponse<BankAccountRequestDTO>.Ok(dto, "Banka hesabı başarıyla eklendi.");
        }
    }
}
