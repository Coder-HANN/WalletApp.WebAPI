using MediatR;
using WalletApp.Application.Feature.DTO;

namespace WalletApp.Application.Feature.Command
{
    public class BankAccountCommand : IRequest<ServiceResponse<BankAccountRequestDTO>>
    {
        public int UserId { get; set; }
        public Guid WalletId { get; set; }
        public string AccountName { get; set; }
        public string Iban { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
    }

}