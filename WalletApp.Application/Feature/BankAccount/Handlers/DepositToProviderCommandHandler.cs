using MediatR;
using WalletApp.Application.Feature.BankAccount.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;

public class DepositToProviderBankCommandHandler : IRequestHandler<DepositToProviderBankAccountRequestDTO, ServiceResponse<DepositToProviderBankAccountResponseDTO>>
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

    public async Task<ServiceResponse<DepositToProviderBankAccountResponseDTO>> Handle(DepositToProviderBankAccountRequestDTO request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<DepositToProviderBankAccountResponseDTO>.Fail("Kullanıcı doğrulanamadı");

        var providerBank = await _providerBankRepository.GetByIdAsync(request.ProviderBankId);
        if (providerBank == null)
            return ServiceResponse<DepositToProviderBankAccountResponseDTO>.Fail("Provider banka bulunamadı.");

        if (request.Amount <= 0)
            return ServiceResponse<DepositToProviderBankAccountResponseDTO>.Fail("Yatırılacak tutar pozitif olmalı.");

        providerBank.TotalBalance += request.Amount;

        await _providerBankRepository.UpdateAsync(providerBank);
        await _providerBankRepository.SaveChangesAsync();

        var response = new DepositToProviderBankAccountResponseDTO
        {
            Iban = request.Iban,
            Amount = request.Amount,
            Description = request.Description 
        };

        return ServiceResponse<DepositToProviderBankAccountResponseDTO>.Ok(response,"Provider banka bakiyesi başarıyla güncellendi.");
    }
}
