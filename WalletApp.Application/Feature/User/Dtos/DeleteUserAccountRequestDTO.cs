using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.Auth.Dtos
{
    public class DeleteUserAccountRequestDTO : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
    }
}
