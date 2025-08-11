using MediatR;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.BankAccount.Handlers
{
    public class ProviderBankAccountCommandHandler: IRequestHandler<ProviderBankAccountCommand, ServiceResponse<ProviderBankAccountResponseDTO>>
    {
        private readonly IProviderBankRepository _providerBankRepository;
        private readonly ICurrentUserService _currentUserService;

        public ProviderBankAccountCommandHandler(
            IProviderBankRepository providerBankRepository,
            ICurrentUserService currentUserService)
        {
            _providerBankRepository = providerBankRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<ProviderBankAccountResponseDTO>> Handle(ProviderBankAccountCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.CurrentUser();
            if (currentUserId == null)
                return ServiceResponse<ProviderBankAccountResponseDTO>.Fail("Kullanıcı doğrulanamadı.");

            var existing = await _providerBankRepository.GetAsync(p => p.BankName == request.BankName);
            if (existing != null)
                return ServiceResponse<ProviderBankAccountResponseDTO>.Fail("Bu banka zaten eklenmiş.");

            // IBAN'dan BankCode çıkarılıyor
            var bankCode = ExtractBankCodeFromIban(request.Iban);
            if (string.IsNullOrEmpty(bankCode))
                return ServiceResponse<ProviderBankAccountResponseDTO>.Fail("Geçersiz IBAN, BankCode alınamadı.");

            var providerBank = new Domain.Entities.ProviderBank
            {
                BankName = request.BankName,
                Iban = request.Iban,
                AccountType = request.AccountType,
                BankCode = bankCode
            };

            await _providerBankRepository.AddAsync(providerBank);
            await _providerBankRepository.SaveChangesAsync();

            var response = new ProviderBankAccountResponseDTO
            {
                BankName = providerBank.BankName,
                Iban = providerBank.Iban,
                AccountType = providerBank.AccountType
            };

            return ServiceResponse<ProviderBankAccountResponseDTO>.Ok(response, "Banka hesabı başarıyla eklendi.");
        }

        private string ExtractBankCodeFromIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban) || iban.Length < 6)
                return null;

            return iban.Substring(5,4); // 5. ve 6. karakterler (index 4 ve 5)
        }

    }
}
