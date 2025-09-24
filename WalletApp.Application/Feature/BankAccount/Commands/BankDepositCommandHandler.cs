using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Bson;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.Abstraction.Services.UnitOfWork;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.BankAccount.Validatiors.Resource;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;


namespace WalletApp.Application.Feature.BankAccount.Handlers
{
    public class BankDepositCommandHandler : IRequestHandler<BankDepositCommand, ServiceResponse<TransactionResponseDTO>>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

        public BankDepositCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<TransactionResponseDTO>> Handle(BankDepositCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.CurrentUser();
            if (currentUserId == null)
                return ServiceResponse<TransactionResponseDTO>.Fail(BankDepositResource.UserIsNotFound);
            // Dış kaynak bankasının Id'si 
            Guid SourceBankId = new Guid("00000000-0000-0000-0000-000000000001"); // dışarıdan para gelince bu değeri veriyoruz

            // Hedef banka kontrolü
            var targetBank = await unitOfWork.BankAccountRepository.GetByIdAsync(request.TargetBankId);
            if (targetBank == null)
                return ServiceResponse<TransactionResponseDTO>.Fail(BankDepositResource.TargetBankNotFound);

            if (request.Amount <= 0)
                return ServiceResponse<TransactionResponseDTO>.Fail(BankDepositResource.AmountMustBeGreaterThanZero);

            // Hedef banka bakiyesini güncelle
            targetBank.Balance += request.Amount;
            await unitOfWork.BankAccountRepository.UpdateAsync(targetBank);

            // Transaction kaydı oluştur
            var transaction = new Transaction
            {
                WalletId = null,
                Amount = request.Amount,
                Type = TransactionType.Deposit,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow
            };
            await unitOfWork.TransactionRepository.AddAsync(transaction);

            // BankTransaction oluştur, source bank dış kaynak, target bank kullanıcı bankası
            var bankTransaction = new BankTransaction
            {
                SourceBankId = null,
                TransactionId = transaction.Id,
                Iban = targetBank.Iban,
                TargetAppBankAccountId = targetBank.Id,
                Commission = "0",
                Transaction = transaction
            };
            await unitOfWork.BankTransactionRepository.AddAsync(bankTransaction);

            await unitOfWork.SaveChangeAsync();

            // Dönüş DTO'su
            var responseDto = new TransactionResponseDTO
            {
                Id = transaction.Id,
                WalletId = null,
                Amount = request.Amount,
                Type = TransactionType.Deposit,
                Description = transaction.Description,
                CreatedDate = transaction.CreatedDate,
                Suggestion = BankDepositResource.SuccessMessage
            };

            return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, BankDepositResource.SuccessMessage);
        }

    }
}