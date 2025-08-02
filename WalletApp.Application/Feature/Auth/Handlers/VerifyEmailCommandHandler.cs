using MediatR;
using Microsoft.Extensions.Caching.Memory;
using WalletApp.Application.Feature.Auth.Dtos;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;


public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailRequestDTO, ServiceResponse<RegisterResponseDTO>>
{
    private readonly IMemoryCache _cache;
    private readonly IEmailService _emailService;

    public VerifyEmailCommandHandler(IMemoryCache cache, IEmailService emailService)
    {
        _cache = cache;
        _emailService = emailService;
    }

    public async Task<ServiceResponse<RegisterResponseDTO>> Handle(VerifyEmailRequestDTO request, CancellationToken cancellationToken)
    {
        // 1. Doğrulama kodu oluştur
        var verificationCode = new Random().Next(100000, 999999).ToString();

        // 2. Doğrulama kodunu cache'e kaydet (örneğin 10 dakika)
        _cache.Set($"email-verification-{request.Email}", verificationCode, TimeSpan.FromMinutes(10));

        // 3. Kullanıcı bilgilerini de cache'e kaydet (json ya da direkt nesne)
        _cache.Set($"pending-register-{request.Email}", request, TimeSpan.FromMinutes(10));

        try
        {
            // 4. Mail gönder
            await _emailService.SendAsync(
                to: request.Email,
                subject: "WalletApp - E-posta Doğrulama Kodu",
                body: $"Doğrulama kodunuz: {verificationCode}");
        }
        catch (Exception ex)
        {
            return ServiceResponse<RegisterResponseDTO>.Fail($"Mail gönderim hatası: {ex.Message}");
        }

        return ServiceResponse<RegisterResponseDTO>.Ok(new RegisterResponseDTO
        {

            Message = "Doğrulama kodu gönderildi. Lütfen e-posta adresinizi kontrol edin."
        });
    }
}
