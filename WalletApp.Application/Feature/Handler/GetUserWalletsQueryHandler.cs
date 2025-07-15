using MediatR;
using WalletApp.Application.Services;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Feature.Queries;


namespace WalletApp.Application.Feature.Handler;

public class GetUserWalletsQueryHandler: IRequestHandler<GetUserWalletsQuery, IEnumerable<CreateWalletDTO>>
{
    private readonly WalletService _walletService;

    public GetUserWalletsQueryHandler(WalletService walletService) => _walletService = walletService;

    public async Task<IEnumerable<CreateWalletDTO>> Handle(GetUserWalletsQuery q,CancellationToken ct)
    {
        var transaction= await _walletService.GetWalletsByUserIdAsync(q.UserId);

        return transaction.Select(t => new CreateWalletDTO
        {
            Id = t.Id,
            UserId = t.UserId,
            Assest = t.Assest,
            TotalBalance = t.TotalBalance,
        });
    }
}
