using WalletApp.Domain.Base;

namespace WalletApp.Application.DTO
{
    public class TransactionRequestDTO
    {
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public Transaction Type { get; set; }  //  enum olarak aldık
        public string? Description { get; set; }
    }
}
