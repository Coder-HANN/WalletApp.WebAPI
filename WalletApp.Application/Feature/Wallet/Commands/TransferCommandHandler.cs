using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.Abstraction.Services.Notification;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Validations.Resource;
using WalletApp.Domain.Enums;


namespace WalletApp.Application.Feature.Wallet.Handlers;

public class TransferCommandHandler : IRequestHandler<TransferCommand, ServiceResponse<TransactionResponseDTO>>
{
    public readonly WalletService _walletService;
    public readonly IHttpContextAccessor _httpContextAccessor;
    public readonly ICurrentUserService _currentUserService;
    public readonly INotificationService _notificationService;

    public TransferCommandHandler(
        WalletService walletService, 
        IHttpContextAccessor httpContextAccessor,
        ICurrentUserService currentUserService,
        INotificationService notificationService)
    {
        _walletService = walletService;
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.CurrentUser();
        if (currentUserId == null || currentUserId == -1)
            return ServiceResponse<TransactionResponseDTO>.Fail(TransferResource.UserIsNotFound);
            var transaction = await _walletService.TransferAsync(request.SourceWalletId, request.TargetWalletId, request.Amount, request.Description.ToString());

        if (transaction == null)
            return ServiceResponse<TransactionResponseDTO>.Fail(TransferResource.FailedMessage);

        await _notificationService.SendToUserAsync(currentUserId.ToString(), TransferResource.PushNotificationMessage);

        return ServiceResponse<TransactionResponseDTO>.Ok(new TransactionResponseDTO
        {
            AppUserId = currentUserId,
            WalletId = request.SourceWalletId,
            Amount = request.Amount,
            Type = TransactionType.Transfer,
        },  TransferResource.SuccessMessage);

        
    }
}
