using MediatR;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Application.Services.EntitiesRepositories;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Wallet.Handlers;

public class DepositCommandHandler : IRequestHandler<DepositRequestDTO, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Constructor - bağımlılıkları alır (Repository ve HttpContext erişimi)
    public DepositCommandHandler(IWalletRepository walletRepository, ITransactionRepository transactionRepository, IHttpContextAccessor httpContextAccessor)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    // Handle metodu: Deposit isteği geldiğinde çalışır
    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(DepositRequestDTO request, CancellationToken cancellationToken)
    {
        // 1. Kullanıcı ID'sini (AppUserId) HttpContext.Items'dan alıyoruz
        //    Eğer yoksa veya parse edilemiyorsa, hata dönüyoruz
        var httpContext = _httpContextAccessor.HttpContext!;
        if (!(_httpContextAccessor.HttpContext?.Items.TryGetValue("AppUserId", out var userIdObj) == true
            && int.TryParse(userIdObj?.ToString(), out var appUserId)))
        {
            return ServiceResponse<TransactionResponseDTO>.Fail("User ID not found in request context.");
        }

        // 2. Gönderilen miktar 0 veya negatif ise hata dön
        if (request.Amount <= 0)
            return ServiceResponse<TransactionResponseDTO>.Fail("Amount must be greater than zero.");

        // 3. İlgili cüzdanı (wallet) veritabanından getir
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
        if (wallet == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Wallet not found.");

        // 4. Cüzdanın sahibi API çağıran kullanıcı mı kontrol et
        if (wallet.AppUserId != appUserId)
            return ServiceResponse<TransactionResponseDTO>.Fail("You do not own this wallet.");

        // 5. Cüzdan bakiyesine yatırılacak miktarı ekle
        wallet.TotalBalance += request.Amount;

        // 6. Güncellenen cüzdan bilgilerini kaydet
        await _walletRepository.UpdateAsync(wallet);

        // 7. Yeni bir işlem (transaction) oluştur ve kaydet
        var transaction = new Transaction
        {
            WalletId = request.WalletId,
            Amount = request.Amount,
            Type = TransactionType.Deposit,
            Description = request.Description ?? "Deposit"
        };
        await _transactionRepository.AddAsync(transaction);

        // 8. Yanıt için DTO oluştur
        var dto = new TransactionResponseDTO
        {
            WalletId = transaction.WalletId,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description
        };

        // 9. Başarılı işlem sonucu dön
        return ServiceResponse<TransactionResponseDTO>.Ok(dto, "Deposit successful.");
    }
}
