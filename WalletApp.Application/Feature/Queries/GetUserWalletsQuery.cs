using MediatR;
using WalletApp.Application.Feature.DTO;
using System;
using System.Collections.Generic;

namespace WalletApp.Application.Feature.Queries
{
    public class GetUserWalletsQuery : IRequest<ServiceResponse<IEnumerable<CreateWalletResponseDTO>>>
    {
        public Guid WalletId { get; set; }
        public int UserId { get; set; }

        public GetUserWalletsQuery(Guid walletId, int userId)
        {
            WalletId = walletId;
            UserId = userId;
        }

        public GetUserWalletsQuery() { }
    }
}
