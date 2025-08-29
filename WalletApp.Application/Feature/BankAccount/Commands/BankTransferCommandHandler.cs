using MediatR;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.BankAccount.Commands;
using WalletApp.Application.Feature.BankAccount.Validatiors.Resource;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Infrastructure.Services.BankServices;

namespace WalletApp.Application.Feature.Wallet.Handlers;

public class BankTransferCommandHandler : IRequestHandler<BankTransferCommand, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IBankRouteRepository _bankRouteRepository;
    private readonly IBankServicesFactory _bankServicesFactory;

    public BankTransferCommandHandler(
        IWalletRepository walletRepository,
        IProviderBankRepository providerBankRepository,
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository,
        IBankTransactionRepository bankTransactionRepository,
        ICurrentUserService currentUser,
        IBankRouteRepository bankRouteRepository,
        IBankServicesFactory bankServicesFactory)
    {
        _walletRepository = walletRepository;
        _providerBankRepository = providerBankRepository;
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _bankTransactionRepository = bankTransactionRepository;
        _currentUser = currentUser;
        _bankRouteRepository = bankRouteRepository;
        _bankServicesFactory = bankServicesFactory;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(BankTransferCommand dto, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.CurrentUser();
        if (currentUserId == null || currentUserId == -1)
            return ServiceResponse<TransactionResponseDTO>.Fail(BankTransferResource.UserIsNotFound);

        var wallet = await _walletRepository.GetAsync(x => x.Id == dto.WalletId && x.AppUserId == currentUserId);
        if (wallet == null)
            return ServiceResponse<TransactionResponseDTO>.Fail(BankTransferResource.WalletIsNotFound);

        if (wallet.TotalBalance < dto.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail(BankTransferResource.WalletAmountIsNotEnough);
                            
        AppBankAccount? targetBankAccount = null;
        string? cleanedIban = null;
        string? targetBankCode = null;
        

        if (dto.RegisterBank == RegisterBank.External)
        {
            cleanedIban = dto.Iban!.Replace(" ", "");
            targetBankCode = cleanedIban.Substring(5, 4);

            targetBankAccount = await _bankAccountRepository.GetAsync(x => x.Iban.Replace(" ", "") == cleanedIban);
            if (targetBankAccount == null)
                return ServiceResponse<TransactionResponseDTO>.Fail(BankTransferResource.BankAccountIsNotFound);
        }
        else if (dto.RegisterBank == RegisterBank.Registered)
        {
            targetBankAccount = await _bankAccountRepository.GetAsync(x => x.Id == dto.TargetBankAccountId);
            if (targetBankAccount == null)
                return ServiceResponse<TransactionResponseDTO>.Fail(BankTransferResource.BankAccountIsNotFound);

            cleanedIban = targetBankAccount.Iban.Replace(" ", "");
            targetBankCode = cleanedIban.Substring(5, 4);
        }
        else
        {
            return ServiceResponse<TransactionResponseDTO>.Fail(BankTransferResource.BankTypeIsNotFound);
        }

        var providerBanks = await _providerBankRepository.GetAllAsync();
        if (providerBanks == null || !providerBanks.Any())
            return ServiceResponse<TransactionResponseDTO>.Fail(BankTransferResource.SourceBankAccountIsNotFound);

        var sourceBankCode = await _bankRouteRepository.GetProviderBankIdAsync(targetBankAccount.Id); 
        var sourceProviderBank = providerBanks.FirstOrDefault(x => x.BankCode == sourceBankCode.ToString());

        if (sourceProviderBank == null)
            return ServiceResponse<TransactionResponseDTO>.Fail(BankTransferResource.ProviderBankAccountIsNotFound);

        var factoryTransfer = _bankServicesFactory.SelectBankServices(sourceProviderBank.BankCode);

        var bankaBakiye = await factoryTransfer.BakiyeBilgisi(dto);

        var transfer = await factoryTransfer.ParaTransferi(dto);


        wallet.TotalBalance -= dto.Amount;
        await _walletRepository.UpdateAsync(wallet);

        targetBankAccount.Balance += dto.Amount;
        await _bankAccountRepository.UpdateAsync(targetBankAccount);

        var transaction = new Transaction
        {
            WalletId = wallet.Id,
            Amount = dto.Amount,
            Type = TransactionType.BankTransfer,
            Description = dto.Description,
            CreatedDate = DateTime.UtcNow
        };
        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();

        var bankTransaction = new BankTransaction
        {
            Id = Guid.NewGuid(),
            TransactionId = transaction.Id,
            ProviderBankId = sourceProviderBank.Id,
            Iban = cleanedIban,
            TargetAppBankAccountId = targetBankAccount.Id,
            Commission = "0",
            SourceBankAccount = null, // Source bank account is not used in this context
        };

        await _bankTransactionRepository.AddAsync(bankTransaction); 
        await _bankTransactionRepository.SaveChangesAsync();


        var responseDto = new TransactionResponseDTO
        {
            AppUserId = currentUserId,
            Id = transaction.Id,
            WalletId = wallet.Id,
            Amount = dto.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            CreatedDate = transaction.CreatedDate
        };
        return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, BankTransferResource.SuccessMessage);
    }
}
