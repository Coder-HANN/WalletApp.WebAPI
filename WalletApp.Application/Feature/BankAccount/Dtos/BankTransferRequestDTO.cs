using MediatR;
using WalletApp.Domain.Enums;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.BankAccount.Commands;

public class BankTransferRequestDTO : IRequest<ServiceResponse<TransactionResponseDTO>>
{
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
    public string? Iban { get; set; }
    public Guid? TargetBankAccountId { get; set; }
    public string? Description { get; set; }
    public RegisterBank RegisterBank { get; set; }
}
