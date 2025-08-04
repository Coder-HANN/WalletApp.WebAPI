using MediatR;
using WalletApp.Application.Feature.BankAccount.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.BankAccount.Handlers;

public class RoutingBankAccountRequestDTOHandler : IRequestHandler<RoutingBankAccountRequestDTO, ServiceResponse<RoutingBankAccountResponseDTO>>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly IBankRouteRepository _bankRouteRepository;

    public RoutingBankAccountRequestDTOHandler(
        IBankAccountRepository bankAccountRepository,
        IProviderBankRepository providerBankRepository,
        IBankRouteRepository bankRouteRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _providerBankRepository = providerBankRepository;
        _bankRouteRepository = bankRouteRepository;
    }

    public async Task<ServiceResponse<RoutingBankAccountResponseDTO>> Handle(RoutingBankAccountRequestDTO request, CancellationToken cancellationToken)
    {
        // IBAN temizle
        var cleanedIban = request.Iban.Replace(" ", "");

        // Banka kodunu IBAN'dan çıkar (örnek: TR15001500001234 → "0015")
        var targetBankCode = cleanedIban.Substring(5, 4);

        // Tüm provider bankaları çek
        var providerBanks = await _providerBankRepository.GetAllAsync();
        if (!providerBanks.Any())
            return ServiceResponse<RoutingBankAccountResponseDTO>.Fail("Provider banka bulunamadı.");

        // Yönlendirme tablosundan uygun provider banka kodunu al
        var sourceBankCode = await _bankRouteRepository.GetProviderBankCodeAsync(targetBankCode);

        // Provider bankayı bul
        var sourceProvider = providerBanks.FirstOrDefault(x => x.BankCode == sourceBankCode);
        if (sourceProvider == null)
            return ServiceResponse<RoutingBankAccountResponseDTO>.Fail("Yönlendirme için uygun provider banka bulunamadı.");

        // Gerekirse hedef banka hesabını da çekebilirsin
        var targetBankAccount = await _bankAccountRepository.GetAsync(x => x.Iban.Replace(" ", "") == cleanedIban);

        var response = new RoutingBankAccountResponseDTO
        {
            SourceProviderBankId = sourceProvider.Id,
            SourceProviderBankName = sourceProvider.BankName,
            TargetBankAccountId = targetBankAccount?.Id,
            TargetBankName = targetBankAccount?.BankName
        };

        return ServiceResponse<RoutingBankAccountResponseDTO>.Ok(response, "Yönlendirme başarıyla tamamlandı.");
    }
}
