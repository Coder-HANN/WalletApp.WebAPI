using MediatR;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;

public class DepositToProviderBankCommandHandler : IRequestHandler<DepositToProviderBankAccountCommand, ServiceResponse<DepositToProviderBankAccountResponseDTO>>
{
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly ICurrentUserService _currentUserService;

    public DepositToProviderBankCommandHandler(
        IProviderBankRepository providerBankRepository,
        ICurrentUserService currentUserService)
    {
        _providerBankRepository = providerBankRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ServiceResponse<DepositToProviderBankAccountResponseDTO>> Handle(DepositToProviderBankAccountCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<DepositToProviderBankAccountResponseDTO>.Fail("Kullanıcı doğrulanamadı.");

        if (string.IsNullOrWhiteSpace(request.Iban))
            return ServiceResponse<DepositToProviderBankAccountResponseDTO>.Fail("IBAN boş olamaz.");

        var providerBank = await _providerBankRepository.GetAsync(p => p.Iban == request.Iban);
        if (providerBank == null)
            return ServiceResponse<DepositToProviderBankAccountResponseDTO>.Fail("IBAN’a ait banka hesabı bulunamadı.");

        if (request.Amount <= 0)
            return ServiceResponse<DepositToProviderBankAccountResponseDTO>.Fail("Yatırılacak tutar pozitif olmalı.");

        providerBank.TotalBalance += request.Amount;

        await _providerBankRepository.UpdateAsync(providerBank);
        await _providerBankRepository.SaveChangesAsync();

        var response = new DepositToProviderBankAccountResponseDTO
        {
            Iban = providerBank.Iban,
            Amount = request.Amount,
            Description = request.Description
        };

        return ServiceResponse<DepositToProviderBankAccountResponseDTO>.Ok(response, "Provider banka bakiyesi başarıyla güncellendi.");
    }

}
