using MediatR;
using Microsoft.AspNetCore.Identity;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace WalletApp.Application.Feature.Auth.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterRequestDTO, ServiceResponse<RegisterResponseDTO>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly WalletService _walletService;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public RegisterUserCommandHandler(
            IWalletRepository walletRepository,
            IPasswordHasher<AppUser> passwordHasher,
            IUserRepository userRepository,
            WalletService walletService,
            IEmailService emailService,
            IMemoryCache cache)
        {
            _walletRepository = walletRepository;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _walletService = walletService;
            _emailService = emailService;
            _cache = cache;
        }

        public async Task<ServiceResponse<RegisterResponseDTO>> Handle(RegisterRequestDTO request, CancellationToken cancellationToken)
        {
            if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
                return ServiceResponse<RegisterResponseDTO>.Fail("Bu e-posta zaten kayıtlı.");

            var user = new AppUser
            {
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(null, request.Password),
                UserDetail = new UserDetail
                {
                    Name = request.Name,
                    BirthDay = request.BirthDay,
                    PhoneNumber = request.PhoneNumber,
                    Occupation = request.Occupation
                }
            };

            try
            {
                _userRepository.Add(user);
                await _userRepository.SaveChangesAsync();

                await _walletService.CreateWalletAsync(user.Id, "TL", cancellationToken);

                // Doğrulama kodu oluştur ve cache'e kaydet
                var code = new Random().Next(100000, 999999).ToString();
                var cacheKey = $"email-verification-{user.Email}";
                _cache.Set(cacheKey, code, TimeSpan.FromMinutes(2));

                // E-posta gönder
                var subject = "WalletApp Doğrulama Kodunuz";
                var body = $"Merhaba {user.UserDetail.Name}," + $"\n\nDoğrulama kodunuz: {code}" + $"\nKod 2 dakika geçerlidir.";

                await _emailService.SendAsync(user.Email, subject, body);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                return ServiceResponse<RegisterResponseDTO>.Fail("Kayıt sırasında hata oluştu: " + inner);
            }

            return ServiceResponse<RegisterResponseDTO>.Ok(new RegisterResponseDTO
            {
                Name = user.UserDetail.Name,
                Email = user.Email,
                Message = "Kayıt başarılı, doğrulama kodu e-posta ile gönderildi."
            }, "Kayıt işlemi tamamlandı.");
        }
    }
}
