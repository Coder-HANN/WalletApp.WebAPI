using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using Microsoft.AspNetCore.Http;
using WalletApp.Domain.Entities;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.Feature.BankAccount.Commands;

namespace WalletApp.Application.Feature.BankAccount.Handlers
{
    public class BankAccountCommandHandler : IRequestHandler<BankAccountCommand, ServiceResponse<BankAccountCommand>>
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IProviderBankRepository _providerBankRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

        public BankAccountCommandHandler(
            IBankAccountRepository bankAccountRepository,
            IProviderBankRepository providerBankRepository,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService)
        {
            _bankAccountRepository = bankAccountRepository;
            _providerBankRepository = providerBankRepository;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;

        }

        public async Task<ServiceResponse<BankAccountCommand>> Handle(BankAccountCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.CurrentUser();
            if (currentUserId == null)
                return ServiceResponse<BankAccountCommand>.Fail("Kullanıcı bulunamadı");

            var bankCode = string.Empty;
            if (!string.IsNullOrEmpty(request.Iban) && request.Iban.Length >= 9)
            {
                bankCode = request.Iban.Substring(5, 4); // IBAN’dan banka kodunu al
            }
            else
            {
                return ServiceResponse<BankAccountCommand>.Fail("Geçersiz IBAN numarası.");
            }

            var entity = new AppBankAccount
            {
                Id = Guid.NewGuid(),
                AppUserId = currentUserId,
                WalletId = request.WalletId,
                AccountName = request.AccountName,
                Iban = request.Iban,
                BankName = request.BankName,
                AccountType = request.AccountType,
                Balance = request.Balance,
                BankCode = bankCode,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _bankAccountRepository.AddAsync(entity);
            await _bankAccountRepository.SaveChangesAsync();

            var dto = new BankAccountCommand
            {
               
                AccountName = entity.AccountName,
                Iban = entity.Iban,
                BankName = entity.BankName,
                AccountType = entity.AccountType,
                Balance = entity.Balance,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                WalletId = entity.WalletId
            };

            return ServiceResponse<BankAccountCommand>.Ok(dto, "Banka hesabı başarıyla eklendi.");
        }

    }
}
