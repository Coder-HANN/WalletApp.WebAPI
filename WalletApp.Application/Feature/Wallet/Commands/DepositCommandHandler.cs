using MediatR;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Feature.Wallet.Commands;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.Feature.Wallet.Validations.Resource;
using WalletApp.Application.Abstraction.Services.Notification;

namespace WalletApp.Application.Feature.Wallet.Handlers;

public class DepositToWalletCommandHandler : IRequestHandler<DepositCommand, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly INotificationService _notificationService;

    public DepositToWalletCommandHandler(
        IWalletRepository walletRepository,
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository,
        IBankTransactionRepository bankTransactionRepository,
        ICurrentUserService currentUserService,
        IProviderBankRepository providerBankRepository,
        INotificationService notificationService)
    {
        _walletRepository = walletRepository;
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _bankTransactionRepository = bankTransactionRepository;
        _currentUserService = currentUserService;
        _providerBankRepository = providerBankRepository;
        _notificationService = notificationService;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.CurrentUser();
        if (currentUserId == null)
            return ServiceResponse<TransactionResponseDTO>.Fail(DepositResource.UserIsNotFound);

        var userBankAccount = await _bankAccountRepository.GetAsync(x => x.Id == request.SourceBankId && x.AppUserId == currentUserId);
        if (userBankAccount == null)
            return ServiceResponse<TransactionResponseDTO>.Fail(DepositResource.BankAccountNotFound);

        if (request.Amount <= 0)
            return ServiceResponse<TransactionResponseDTO>.Fail(DepositResource.AmountMustBeGreaterThanZero);

        if (userBankAccount.Balance < request.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail(DepositResource.InsufficientBalance);

        var wallet = await _walletRepository.GetAsync(x => x.Id == request.WalletId && x.AppUserId == currentUserId);
        if (wallet == null)
            return ServiceResponse<TransactionResponseDTO>.Fail(DepositResource.WalletNotFound);

        // IBAN'dan banka kodunu al (boşlukları kaldır, 5. ve 6. karakterler)
        
        if (userBankAccount.Iban.Length < 24)
            return ServiceResponse<TransactionResponseDTO>.Fail(DepositResource.InavliedIban);

        var bankCode = userBankAccount.Iban.Substring(5, 4);

        var providerBanks = await _providerBankRepository.GetAllAsync();
        var providerBank = providerBanks.FirstOrDefault(pb => pb.BankCode == bankCode);

        if (providerBank == null)
        {
            // VakıfBank (0015) default olarak atanabilir
            providerBank = providerBanks.FirstOrDefault(pb => pb.BankCode == "0015");
            if (providerBank == null)
                return ServiceResponse<TransactionResponseDTO>.Fail(DepositResource.ProviderBankAccountNotFound);
        }

        // Güncellemeler
        userBankAccount.Balance -= request.Amount;
        await _bankAccountRepository.UpdateAsync(userBankAccount);

        providerBank.TotalBalance += request.Amount;
        await _providerBankRepository.UpdateAsync(providerBank);

        wallet.TotalBalance += request.Amount;
        await _walletRepository.UpdateAsync(wallet);

        // Transaction kaydı
        var transaction = new Transaction
        {
            WalletId = wallet.Id,
            Amount = request.Amount,
            Type = TransactionType.Deposit,
            Description = request.Description,
            CreatedDate = DateTime.UtcNow
        };
        await _transactionRepository.AddAsync(transaction);

        // BankTransaction kaydı
        var bankTransaction = new BankTransaction
        {
            TransactionId = transaction.Id,
            ProviderBankId = providerBank.Id,
            SourceBankId = userBankAccount.Id,
            Iban = userBankAccount.Iban,
            Commission = "0",
            Transaction = transaction
        };
        await _bankTransactionRepository.AddAsync(bankTransaction);

        var responseDto = new TransactionResponseDTO
        {
            Id = transaction.Id,
            WalletId = wallet.Id,
            Amount = request.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            CreatedDate = transaction.CreatedDate,
            Suggestion = DepositResource.SuccessMessage
        };
        await _notificationService.SendToUserAsync(currentUserId.ToString(), DepositResource.PushNotificationMessage);

        return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, DepositResource.SuccessMessage);
    }

}