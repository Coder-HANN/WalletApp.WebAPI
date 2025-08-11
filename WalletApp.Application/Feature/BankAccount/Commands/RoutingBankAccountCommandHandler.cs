using MediatR;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.BankAccount.Handlers;

public class RoutingBankAccountCommandHandler : IRequestHandler<RoutingBankAccountCommand, ServiceResponse<RoutingBankAccountResponseDTO>>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly IBankRouteRepository _bankRouteRepository;
    private readonly ICurrentUserService _currentUserService;

    public RoutingBankAccountCommandHandler(
        IBankAccountRepository bankAccountRepository,
        IProviderBankRepository providerBankRepository,
        IBankRouteRepository bankRouteRepository,
        ICurrentUserService currentUserService)
    {
        _bankAccountRepository = bankAccountRepository;
        _providerBankRepository = providerBankRepository;
        _bankRouteRepository = bankRouteRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ServiceResponse<RoutingBankAccountResponseDTO>> Handle(RoutingBankAccountCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<RoutingBankAccountResponseDTO>.Fail("Kullanıcı doğrulanamadı");
        
        var cleanedIban = request.Iban.Replace(" ", "");

       
        var targetBankCode = cleanedIban.Substring(5, 4);

        
        var providerBanks = await _providerBankRepository.GetAllAsync();
        if (!providerBanks.Any())
            return ServiceResponse<RoutingBankAccountResponseDTO>.Fail("Provider banka bulunamadı.");

        // Yönlendirme tablosundan uygun provider banka kodunu al
        var sourceBankCode = await _bankRouteRepository.GetProviderBankCodeAsync(targetBankCode);

        
        var sourceProvider = providerBanks.FirstOrDefault(x => x.BankCode == sourceBankCode);
        if (sourceProvider == null)
            return ServiceResponse<RoutingBankAccountResponseDTO>.Fail("Yönlendirme için uygun provider banka bulunamadı.");

        
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
