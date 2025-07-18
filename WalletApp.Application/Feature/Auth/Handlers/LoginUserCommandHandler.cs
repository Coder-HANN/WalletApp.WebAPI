using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;
using WalletApp.Application.Services.Repositories;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.User.Dtos;


namespace WalletApp.Application.Feature.Auth.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserRequestDTO, ServiceResponse<LoginUserResponseDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IConfiguration _configuration;
        


        private readonly IEntityRepository<AppUser> _entityRepository;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher<AppUser> passwordHasher,
            IConfiguration configuration,
            IEntityRepository<AppUser> entityRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _entityRepository = entityRepository;
        }

        public async Task<ServiceResponse<LoginUserResponseDTO>> Handle(LoginUserRequestDTO request, CancellationToken cancellationToken)
        {
            var user = _entityRepository.Query().FirstOrDefault(u => u.Email == request.RequestDTO.Email);
            if (user == null)
                return ServiceResponse<LoginUserResponseDTO>.Fail("Email veya şifre hatalı.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.RequestDTO.Password);
            if (result == PasswordVerificationResult.Failed)
                return ServiceResponse<LoginUserResponseDTO>.Fail("Email veya şifre hatalı.");

            var token = GenerateJwtToken(user); // Fixed: Removed the incorrect 'JwtRegisteredClaimNames' argument
            var expiration = DateTime.UtcNow.AddHours(1);

            return ServiceResponse<LoginUserResponseDTO>.Ok(new LoginUserResponseDTO
            {
                Token = token,
                Email = user.Email,
                AppUserId = user.Id,
                TokenExpiration = expiration
            }, "Giriş başarılı.");
        }

        private string GenerateJwtToken(AppUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("email", user.Email),
                new Claim("AppUserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
