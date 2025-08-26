namespace WalletApp.Application.DTOs.BankAccount
{
    public class RoutingBankAccountResponseDTO 
    {
        public Guid? SourceProviderBankId { get; set; }
        public Guid? TargetBankAccountId { get; set; }
    }
}
