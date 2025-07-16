using MediatR;
using Microsoft.AspNetCore.Identity;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Services.Repositories.EntitysRepository;
using WalletApp.Domain.Base;


namespace WalletApp.Application.Feature.Handler
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ServiceResponse<RegisterResponseDTO>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly WalletService _walletService;
        
        public RegisterUserCommandHandler(
            IWalletRepository walletRepository,
            IPasswordHasher<User> passwordHasher,
            IUserRepository userRepository,
            WalletService walletService)
        {
            _walletRepository = walletRepository;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _walletService = walletService;
        }

        public async Task<ServiceResponse<RegisterResponseDTO>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RegisterDTO;

            if (await _userRepository.EmailExistsAsync(dto.Email, cancellationToken))
                return ServiceResponse<RegisterResponseDTO>.Fail("Bu e-posta zaten kayıtlı.");

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(null, dto.Password),
                UserDetail = new UserDetail
                {
                    Name = dto.Name,
                    BirthDay = dto.BirthDay,
                    PhoneNumber = dto.PhoneNumber,
                    Occupation = dto.Occupation
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

