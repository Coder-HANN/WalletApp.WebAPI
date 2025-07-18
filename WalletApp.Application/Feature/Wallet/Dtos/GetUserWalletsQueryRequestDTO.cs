using MediatR;
using System;
using System.Collections.Generic;
using WalletApp.Application.Feature.Wallet.Dtos;

public class GetUserWalletsQueryRequestDTO: IRequest<ServiceResponse<IEnumerable<AppWalletResponseDTO>>>
{
    public Guid WalletId { get; set; }
    public int UserId { get; set; }

    public GetUserWalletsQueryRequestDTO(Guid walletId, int userId)
    {
        WalletId = walletId;
        UserId = userId;
    }
}
