using MediatR;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.BankAccount.Validatiors.Resource;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;


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
                return ServiceResponse<ProviderBankAccountResponseDTO>.Fail(ProviderBankAccountResource.UserIsNotFound);

            // IBAN'dan BankCode çıkarılıyor
            var bankCode = ExtractBankCodeFromIban(request.Iban);
            if (string.IsNullOrEmpty(bankCode))
                return ServiceResponse<ProviderBankAccountResponseDTO>.Fail(ProviderBankAccountResource.InvaliedIban);

            var existing = await _providerBankRepository.GetAsync(p => p.BankCode == bankCode);
            if (existing != null)
                return ServiceResponse<ProviderBankAccountResponseDTO>.Fail(ProviderBankAccountResource.BankAccountIsAvailable);

            var providerBank = new ProviderBank
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

            return ServiceResponse<ProviderBankAccountResponseDTO>.Ok(response, ProviderBankAccountResource.SuccessMessage);
        }

        private string ExtractBankCodeFromIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban) || iban.Length < 26)
                return null;

            return iban.Replace(" "," ").Substring(5,4); 
        }

    }
}
