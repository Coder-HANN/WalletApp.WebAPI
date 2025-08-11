using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Application.Feature.Wallet.Dtos;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.Feature.Wallet.Commands;

public class AppWalletCommandHandler : IRequestHandler<AppWalletCommand, ServiceResponse<AppWalletResponseDTO>>
{
    private readonly WalletService _walletService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentUserService _currentUserService;

	public AppWalletCommandHandler(
        WalletService walletService,
        IHttpContextAccessor httpContextAccessor,
        ICurrentUserService currentUserService)
    {
        _walletService = walletService;
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
	}

    public async Task<ServiceResponse<AppWalletResponseDTO>> Handle(AppWalletCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<AppWalletResponseDTO>.Fail("Kullanıcı bulunamadı");

		if (string.IsNullOrEmpty(request.Name))
            return ServiceResponse<AppWalletResponseDTO>.Fail("Cüzdan adı boş olamaz.");

        var result = await _walletService.CreateWalletAsync( request.Currency, cancellationToken);

        if (result == null)
            return ServiceResponse<AppWalletResponseDTO>.Fail("Cüzdan oluşturulamadı.");

        var dto = new AppWalletResponseDTO
        {
            Id = result.Id,
            AppUserId = currentUserId,
            TotalBalance = result.TotalBalance,
            Assest = result.Assest 
        };

        return ServiceResponse<AppWalletResponseDTO>.Ok(dto, "Cüzdan başarıyla oluşturuldu.");
    }
}