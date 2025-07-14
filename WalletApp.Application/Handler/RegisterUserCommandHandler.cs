using MediatR;
using Microsoft.AspNetCore.Identity;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Application.Command;
using WalletApp.Application.DTO;
using WalletApp.Application.Services;
using WalletApp.Domain.Base;


namespace WalletApp.Application.Handler.RegisterUserCommandHandler
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponseDTO>
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

        public async Task<RegisterResponseDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RegisterDTO;

            var emailExists = await _userRepository.EmailExistsAsync(dto.Email, cancellationToken);
            if (emailExists)
                throw new Exception("Bu e-posta zaten kayıtlı.");

            // User ve UserDetail ilişkilendirilmiş şekilde oluşturuluyor
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
                await _walletService.CreateWalletAsync(user.Id, "TL",CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Gerçek hata inner exception’da olabilir, varsa onu göster
                var inner = ex.InnerException?.Message ?? ex.Message;
                throw new Exception("Kayıt sırasında hata oluştu: " + inner);
            }

            return new RegisterResponseDTO
            {
                Name = user.UserDetail.Name,
                Email = user.Email,
                Message = "Kayıt başarılı",
                
            };
        }
    }
}
