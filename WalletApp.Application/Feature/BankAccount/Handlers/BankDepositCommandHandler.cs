using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Feature.BankAccount.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;


namespace WalletApp.Application.Feature.BankAccount.Handlers
{
    public class BankDepositCommandHandler : IRequestHandler<BankDepositRequestDTO, ServiceResponse<TransactionResponseDTO>>
    {
        
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankTransactionRepository _bankTransactionRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankDepositCommandHandler(
            IBankTransactionRepository bankTransactionRepository,
            IBankAccountRepository bankAccountRepository,
            ITransactionRepository transactionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bankTransactionRepository = bankTransactionRepository;
            _bankAccountRepository = bankAccountRepository;
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<TransactionResponseDTO>> Handle(BankDepositRequestDTO request, CancellationToken cancellationToken)
        {
            // Dış kaynak bankasının Id'si 
            Guid SourceBankId = new Guid("00000000-0000-0000-0000-000000000001"); // dışarıdan para gelince bu değeri veriyoruz

            // Hedef banka kontrolü
            var targetBank = await _bankAccountRepository.GetByIdAsync(request.TargetBankId);
            if (targetBank == null)
                return ServiceResponse<TransactionResponseDTO>.Fail("Target bank not found.");

            if (request.Amount <= 0)
                return ServiceResponse<TransactionResponseDTO>.Fail("Amount must be greater than zero.");

            // Hedef banka bakiyesini güncelle
            targetBank.Balance += request.Amount;
            await _bankAccountRepository.UpdateAsync(targetBank);

            // Transaction kaydı oluştur
            var transaction = new Transaction
            {
                WalletId = null,
                Amount = request.Amount,
                Type = TransactionType.Deposit,
                Description = request.Description ?? "External deposit",
                CreatedDate = DateTime.UtcNow
            };
            await _transactionRepository.AddAsync(transaction);

            // BankTransaction oluştur, source bank dış kaynak, target bank kullanıcı bankası
            var bankTransaction = new BankTransaction
            {
                TransactionId = transaction.Id,
                ProviderBankId = targetBank.ProviderBankId,
                Iban = targetBank.Iban,
                TargetBankId = targetBank.Id,
                Commission = "0",
                Transaction = transaction
            };
            await _bankTransactionRepository.AddAsync(bankTransaction);

            // Dönüş DTO'su
            var responseDto = new TransactionResponseDTO
            {
                Id = transaction.Id,
                WalletId = null,
                Amount = request.Amount,
                Type = TransactionType.Deposit,
                Description = transaction.Description,
                CreatedDate = transaction.CreatedDate,
                Suggestion = "Deposit from external source"
            };

            return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, "Deposit successful.");
        }

    }
}