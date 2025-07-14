using MediatR;
using WalletApp.Application.Command.CreateWalletCommand;
using WalletApp.Application.DTO;
using WalletApp.Application.Services;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, ServiceResponse<CreateWalletDTO>>
{
    private readonly WalletService _walletService;

    public CreateWalletCommandHandler(WalletService walletService)
    {
        _walletService = walletService;
    }

    public async Task<ServiceResponse<CreateWalletDTO>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name))
            return ServiceResponse<CreateWalletDTO>.Fail("Cüzdan adı boş olamaz.");

        var result = await _walletService.CreateWalletAsync(request.UserId, request.Currency, cancellationToken);

        if (result == null)
            return ServiceResponse<CreateWalletDTO>.Fail("Cüzdan oluşturulamadı.");

        var dto = new CreateWalletDTO
        {
            Id = result.Id,
            UserId = result.UserId,
            Currency = result.Currency,
            TotalBalance = result.TotalBalance
        };

        return ServiceResponse<CreateWalletDTO>.Ok(dto, "Cüzdan başarıyla oluşturuldu.");
    }
}