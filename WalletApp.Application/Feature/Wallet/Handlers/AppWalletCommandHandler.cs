using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Application.Feature.Wallet.Dtos;
using Microsoft.AspNetCore.Http;

public class AppWalletCommandHandler : IRequestHandler<AppWalletRequestDTO, ServiceResponse<AppWalletResponseDTO>>
{
    private readonly WalletService _walletService;
    private readonly IHttpContextAccessor _httpContextAccessor;

	public AppWalletCommandHandler(WalletService walletService,IHttpContextAccessor httpContextAccessor)
    {
        _walletService = walletService;
        _httpContextAccessor = httpContextAccessor;
	}

    public async Task<ServiceResponse<AppWalletResponseDTO>> Handle(AppWalletRequestDTO request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("AppUserId");
        if (userIdClaim == null)
            return ServiceResponse<AppWalletResponseDTO>.Fail("Kullanıcı kimliği bulunamadı.");

		if (string.IsNullOrEmpty(request.Name))
            return ServiceResponse<AppWalletResponseDTO>.Fail("Cüzdan adı boş olamaz.");

        var result = await _walletService.CreateWalletAsync(request.AppUserId, request.Currency, cancellationToken);

        if (result == null)
            return ServiceResponse<AppWalletResponseDTO>.Fail("Cüzdan oluşturulamadı.");

        var dto = new AppWalletResponseDTO
        {
            Id = result.Id,
            AppUserId = result.AppUserId,
            TotalBalance = result.TotalBalance,
            Assest = result.Assest 
        };

        return ServiceResponse<AppWalletResponseDTO>.Ok(dto, "Cüzdan başarıyla oluşturuldu.");
    }
}