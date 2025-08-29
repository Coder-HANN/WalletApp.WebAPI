using MediatR;
using System.Data.Entity;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.DTOs.BankAccount;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.BankAccount.Validatiors.Resource;
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
            return ServiceResponse<RoutingBankAccountResponseDTO>.Fail(RoutingBankAccountResource.UserIsNotFound);

        //TODO : Source ve target, provider tablosunda var mı? Target tüm seçeneği için olmasa da olur.

        //TODO : Source-Target kaydı var mı?

        var isSourceBankAccount = await _providerBankRepository.Query().AnyAsync(x => x.Id == request.SourceProviderBankId);

        if (!isSourceBankAccount)
            return ServiceResponse<RoutingBankAccountResponseDTO>.Fail(RoutingBankAccountResource.SourceBankAccountIsNotFound);

        var isTargetBankAccount = await _providerBankRepository.Query().AnyAsync(x => x.Id == request.TargetProviderBankId);

        if (!isTargetBankAccount)
        {
            var isAllTarget = await _bankRouteRepository.Query().AnyAsync(x => x.TargetBankId == null);

            if (isAllTarget)
            {
                throw new Exception(RoutingBankAccountResource.RoutingAvailable);
            }
        }

        var routingRegister = await _bankRouteRepository.Query().AnyAsync(x => x.TargetBankId == request.TargetProviderBankId);

        if (routingRegister != null)
        {
            var result = new ServiceResponse<RoutingBankAccountResponseDTO>()
            {
                Message = RoutingBankAccountResource.RoutingAvailableForThisBank,
                Success = false
            };

            return result;
        }

        var entity = new BankRoute
        {
            TargetBankId = request.TargetProviderBankId,
            SourceBankId = request.SourceProviderBankId,
            Remark = request.ToString(),
        };

        await _bankRouteRepository.AddAsync(entity);
        await _bankRouteRepository.SaveChangesAsync();

        var response = new RoutingBankAccountResponseDTO
        {
            SourceProviderBankId = request.SourceProviderBankId,
            TargetBankAccountId = request.TargetProviderBankId,
        };

        return ServiceResponse<RoutingBankAccountResponseDTO>.Ok(response, RoutingBankAccountResource.SuccessMessage);

    }
}
