using MediatR;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.BankAccount.Handlers;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Application.Feature.Wallet.Dtos;
using Microsoft.AspNetCore.Http;


namespace WalletApp.Application.Feature.BankAccount.Handlers
{
    public class BankTransferCommandHandler : IRequestHandler<BankTransferRequestDTO, ServiceResponse<TransactionResponseDTO>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _TransactionRepository;
        private readonly IBankTransactionRepository _bankTransactionRepository;
        private readonly IProviderBankRepository _providerBankRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankTransferCommandHandler(
            IWalletRepository walletRepository,
            ITransactionRepository TransactionRepository,
            IBankTransactionRepository bankTransactionRepository,
            IProviderBankRepository providerBankRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _walletRepository = walletRepository;
            _TransactionRepository = TransactionRepository;
            _bankTransactionRepository = bankTransactionRepository;
            _providerBankRepository = providerBankRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<TransactionResponseDTO>> Handle(BankTransferRequestDTO request, CancellationToken cancellationToken)
        {
            
            var dto = request;

            var wallet = await _walletRepository.GetAsync(w => w.Id == dto.WalletId);
            if (wallet == null)
                return ServiceResponse<TransactionResponseDTO>.Fail("Cüzdan bulunamadı");

            if (wallet.TotalBalance < dto.Amount)
                return ServiceResponse<TransactionResponseDTO>.Fail("Yetersiz bakiye");

            var providerBank = await _providerBankRepository.GetAsync(p => p.Id == dto.ProviderBankId);
            if (providerBank == null)
                return ServiceResponse<TransactionResponseDTO>.Fail("Sağlayıcı banka bulunamadı");

            wallet.TotalBalance -= dto.Amount;
            await _walletRepository.UpdateAsync(wallet);

            var transaction = new Transaction
            {
                WalletId = wallet.Id,
                Amount = dto.Amount,
                Type = TransactionType.BankTransfer,
                Currency = 0,
                Description = dto.Description ?? $"Banka transferi - {dto.Iban}"
            };
            await _TransactionRepository.AddAsync(transaction);

            var bankTransaction = new BankTransaction
            {
                TransactionId = transaction.Id,
                ProviderBankId = providerBank.Id,
                Iban = dto.Iban,
                TargetBankId = dto.TargetBankId,
                SourceBankId = dto.SourceBankId,
                Commission = "0"
            };
            await _bankTransactionRepository.AddAsync(bankTransaction);

            var responseDto = new TransactionResponseDTO
            {
                WalletId = transaction.WalletId,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Description = transaction.Description
            };

            return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, "Banka transferi başarılı.");
        }
    }
}
