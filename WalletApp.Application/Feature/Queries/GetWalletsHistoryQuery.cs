using MediatR;
using WalletApp.Application.Feature.DTO;
using System;
using System.Collections.Generic;

namespace WalletApp.Application.Feature.Queries
{
    public class GetWalletHistoryQuery : IRequest<ServiceResponse<IEnumerable<TransactionResponseDTO>>>
    {
        public Guid WalletId { get; }
        public int UserId { get; }

        public GetWalletHistoryQuery(Guid walletId, int userId)
        {
            WalletId = walletId;
            UserId = userId;
        }
    }
}
