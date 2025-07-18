﻿using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;


namespace WalletApp.Application.Feature.BankAccount.Commands
{
    public class BankTransferRequestDTO : IRequest<ServiceResponse<TransactionResponseDTO>>
   
    {
        public Guid WalletId { get; set; }
        public Guid ProviderBankId { get; set; }
        public Guid TargetBankId { get; set; }
        public Guid SourceBankId { get; set; }
        public int Iban { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

}
