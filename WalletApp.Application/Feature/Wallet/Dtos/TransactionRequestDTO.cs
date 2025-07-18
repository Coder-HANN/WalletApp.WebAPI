using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using MediatR;

namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public class TransactionRequestDTO : IRequest<ServiceResponse<TransactionResponseDTO>>
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }  //  enum olarak aldık
        public string? Description { get; set; }
    }
}
