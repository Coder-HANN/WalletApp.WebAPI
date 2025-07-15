using WalletApp.Domain.Base;
using WalletApp.Domain.Enums;

namespace WalletApp.Application.Feature.DTO
{
    public class TransactionRequestDTO
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }  //  enum olarak aldık
        public string? Description { get; set; }
    }
}
