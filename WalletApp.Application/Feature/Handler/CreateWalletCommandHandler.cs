using MediatR;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Feature.Handler;
using System.Threading;
using System.Threading.Tasks;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, ServiceResponse<CreateWalletResponseDTO>>
{
    private readonly WalletService _walletService;

    public CreateWalletCommandHandler(WalletService walletService)
    {
        _walletService = walletService;
    }

    public async Task<ServiceResponse<CreateWalletResponseDTO>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name))
            return ServiceResponse<CreateWalletResponseDTO>.Fail("Cüzdan adı boş olamaz.");

        var result = await _walletService.CreateWalletAsync(request.UserId, request.Currency, cancellationToken);

        if (result == null)
            return ServiceResponse<CreateWalletResponseDTO>.Fail("Cüzdan oluşturulamadı.");

        var dto = new CreateWalletResponseDTO
        {
            Id = result.Id,
            UserId = result.UserId,
            Currency = result.Currency,
            TotalBalance = result.TotalBalance
        };

        return ServiceResponse<CreateWalletResponseDTO>.Ok(dto, "Cüzdan başarıyla oluşturuldu.");
    }
}