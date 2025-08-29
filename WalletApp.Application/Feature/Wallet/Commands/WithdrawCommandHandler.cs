using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;

public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IProviderBankRepository _providerBankRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentUserService _currentUserService;

    public WithdrawCommandHandler(
        IWalletRepository walletRepository,
        ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository,
        IProviderBankRepository providerBankRepository,
        IBankTransactionRepository bankTransactionRepository,
        IHttpContextAccessor httpContextAccessor,
        ICurrentUserService currentUserService)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
        _providerBankRepository = providerBankRepository;
        _bankTransactionRepository = bankTransactionRepository;
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
    }


    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        // Kullanıcı doğrulama
        var currentUserId = _currentUserService.CurrentUser();
        if (currentUserId == null || currentUserId == -1)
            return ServiceResponse<TransactionResponseDTO>.Fail("Kullanıcı doğrulanamadı.");

        //  Cüzdan kontrolü
        var wallet = await _walletRepository.GetAsync(x => x.Id == request.WalletId && x.AppUserId == currentUserId);
        if (wallet == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Cüzdan bulunamadı.");

        if (wallet.TotalBalance < request.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Cüzdan bakiyesi yetersiz.");

        // Kullanıcı banka hesabı kontrolü
        var userBankAccount = await _bankAccountRepository.GetAsync(
            x => x.Id == request.AppBankAccountId && x.AppUserId == currentUserId);
        if (userBankAccount == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Banka hesabı bulunamadı veya size ait değil.");

        //  IBAN’dan banka kodu çıkar
        var cleanIban = userBankAccount.Iban.Replace(" ", "");
        if (cleanIban.Length < 9)
            return ServiceResponse<TransactionResponseDTO>.Fail("Geçersiz IBAN.");
        var bankCode = cleanIban.Substring(5, 4);

        //  Provider bank seçimi (önce aynısı, yoksa 0015 VakıfBank)
        var providerBanks = await _providerBankRepository.GetAllAsync();

        var providerBank = providerBanks.FirstOrDefault(pb => pb.BankCode == bankCode) ?? providerBanks.FirstOrDefault(pb => pb.BankCode == "0015");

        var providerBankAccount = await _bankAccountRepository.GetAsync(x => x.ProviderBankId == providerBank.Id);

        if (providerBank == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Provider banka bulunamadı.");

        if (providerBank.TotalBalance < request.Amount)
            return ServiceResponse<TransactionResponseDTO>.Fail("Provider banka bakiyesi yetersiz.");
        
        wallet.TotalBalance -= request.Amount;
        await _walletRepository.UpdateAsync(wallet);

        // Provider banktan gerçek düşüş
        providerBank.TotalBalance -= request.Amount;
        await _providerBankRepository.UpdateAsync(providerBank);

        // Transaction kaydı
        var transaction = new Transaction
        {
            WalletId = wallet.Id,
            Amount = request.Amount,
            Type = TransactionType.Withdraw,
            Description = request.Description,
            CreatedDate = DateTime.UtcNow,
            AppBankAccountId = request.AppBankAccountId
        };
        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();
        
        var responseDto = new TransactionResponseDTO
        {
            Id = transaction.Id,
            AppUserId = currentUserId,
            WalletId = wallet.Id,
            Amount = request.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            CreatedDate = transaction.CreatedDate
        };

        return ServiceResponse<TransactionResponseDTO>.Ok(responseDto, "Para çekme işlemi başarılı.");
    }

}
