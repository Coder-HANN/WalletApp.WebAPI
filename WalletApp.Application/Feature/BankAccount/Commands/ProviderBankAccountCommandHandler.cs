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

        public async Task<ServiceResponse<ProviderBankAccountResponseDTO>> Handle(
            ProviderBankAccountCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.CurrentUser();
            if (currentUserId == null)
                return ServiceResponse<ProviderBankAccountResponseDTO>.Fail("Kullanıcı doğrulanamadı.");

            var providerBank = await _providerBankRepository.GetAsync(p => p.BankName == request.BankName);
            if (providerBank == null)
                return ServiceResponse<ProviderBankAccountResponseDTO>.Fail("Banka bulunamadı.");

            providerBank.BankName = request.BankName;
            providerBank.Iban = request.Iban;
            providerBank.AccountType = request.AccountType;


            await _providerBankRepository.UpdateAsync(providerBank);
            await _providerBankRepository.SaveChangesAsync();

            var response = new ProviderBankAccountResponseDTO
            {
                BankName = providerBank.BankName,
                Iban = providerBank.Iban,
                AccountType = providerBank.AccountType
            };

            return ServiceResponse<ProviderBankAccountResponseDTO>.Ok(response, "Banka hesabı başarıyla güncellendi.");
        }
    }
}
