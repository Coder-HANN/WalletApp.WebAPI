﻿namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public class DepositResponseDTO
    {
        public Guid WalletId { get; set; }
        public decimal NewBalance { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }
    }
}
