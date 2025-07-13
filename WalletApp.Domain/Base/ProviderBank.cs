

namespace WalletApp.Domain.Base
{
    public class ProviderBank : ProductClass
    {
        public int Id { get; set; }
        public string BankName { get; set; }

        public ICollection<BankTransaction> BankTransactions { get; set; }
    }
}
