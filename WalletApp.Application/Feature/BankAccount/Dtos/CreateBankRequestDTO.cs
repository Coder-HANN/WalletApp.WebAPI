namespace WalletApp.Application.Feature.BankAccount.Commands;
    public class CreateBankAccountRequestDTO
{
    public Guid WalletId { get; set; }
    public string Bilgiler { get; set; }
}
