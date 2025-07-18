using MediatR;
using Microsoft.AspNetCore.Identity;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.User.Dtos;


namespace WalletApp.Application.Feature.Auth.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterRequestDTO, ServiceResponse<RegisterResponseDTO>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly WalletService _walletService;
        
        public RegisterUserCommandHandler(
            IWalletRepository walletRepository,
            IPasswordHasher<AppUser> passwordHasher,
            IUserRepository userRepository,
            WalletService walletService)
        {
            _walletRepository = walletRepository;
            _passwordHasher = passwordHasher;  
            _userRepository = userRepository;
            _walletService = walletService;
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
                await _walletService.CreateWalletAsync(user.Id, "TL", CancellationToken.None);
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
                Message = "Kayıt başarılı"
            }, "Kayıt işlemi tamamlandı.");
        }
    }
}

