using MediatR;
using Microsoft.Extensions.Caching.Memory;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;

public class RegisterUserCommandHandler : IRequestHandler<RegisterRequestDTO, ServiceResponse<RegisterResponseDTO>>
{
    private readonly IMemoryCache _cache;
    private readonly IUserDetailRepository _userDetailRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(
        IMemoryCache cache,
        IUserDetailRepository userDetailRepository,
        IUserRepository userRepository,
        IEmailService emailService)
    {
        _cache = cache;
        _userDetailRepository = userDetailRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }
  
    public async Task<ServiceResponse<RegisterResponseDTO>> Handle(RegisterRequestDTO request, CancellationToken cancellationToken)
    {
        var codeKey = $"email-verification-{request.Email}";
        var dataKey = $"pending-register-{request.Email}";

        // Eğer doğrulama kodu boşsa → doğrulama kodu gönder
        if (string.IsNullOrWhiteSpace(request.VerificationCode))
        {
            // Doğrulama kodu üret
            var verificationCode = new Random().Next(100000, 999999).ToString();

            // Kod ve kullanıcı bilgilerini cache’e kaydet (örneğin 10 dakika)
            _cache.Set(codeKey, verificationCode, TimeSpan.FromMinutes(10));
            _cache.Set(dataKey, request, TimeSpan.FromMinutes(10));

            try
            {
                // Doğrulama kodunu mail olarak gönder
                await _emailService.SendAsync(
                    to: request.Email,
                    subject: "WalletApp - Doğrulama Kodu",
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

        // Doğrulama kodu gelmiş, kontrol et
        if (!_cache.TryGetValue(codeKey, out string? expectedCode) || expectedCode != request.VerificationCode)
        {
            return ServiceResponse<RegisterResponseDTO>.Fail("Geçersiz veya süresi dolmuş doğrulama kodu.");
        }

        // Cache’den kayıt verisini al
        if (!_cache.TryGetValue(dataKey, out RegisterRequestDTO? pendingRegister))
        {
            return ServiceResponse<RegisterResponseDTO>.Fail("Kayıt verisi bulunamadı veya süresi doldu.");
        }

        // Cache temizle
        _cache.Remove(codeKey);
        _cache.Remove(dataKey);

        // Kayıt işlemi
        await _userDetailRepository.AddAsync(new UserDetail
        {
            Name = pendingRegister.Name,
            Surname = pendingRegister.Surname,
            Occupation = pendingRegister.Occupation,
            Address = pendingRegister.Address,
            PhoneNumber = pendingRegister.PhoneNumber,
            Gender = pendingRegister.Gender,
            BirthDay = pendingRegister.BirthDay
        });

        await _userRepository.AddAsync(new AppUser
        {
            Email = pendingRegister.Email,
            PasswordHash = pendingRegister.Password, // Burada hashlemeyi unutma!
            EmailConfirmed = true
        });

        return ServiceResponse<RegisterResponseDTO>.Ok(new RegisterResponseDTO
        {
            Email = pendingRegister.Email,
            Name = pendingRegister.Name,
            Surname = pendingRegister.Surname,
            Message = "Kayıt başarılı."
        }, "Kayıt tamamlandı.");
    }
}
