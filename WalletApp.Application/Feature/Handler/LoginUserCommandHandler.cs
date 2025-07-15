using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WalletApp.Domain.Base;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Services.Repositories;
using WalletApp.Application.Services.Repositories.EntitysRepository;

namespace WalletApp.Application.Feature.Handler
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand,LoginResponseDTO >
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;


        private readonly IEntityRepository<User> _entityRepository;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IConfiguration configuration,
            IEntityRepository<User> entityRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _entityRepository = entityRepository;
        }

        public async Task<LoginResponseDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = _entityRepository.Query()
                .FirstOrDefault(u => u.Email == request.RequestDTO.Email);

            if (user == null)
                throw new Exception("Email veya şifre hatalı.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.RequestDTO.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Email veya şifre hatalı.");

            var token = GenerateJwtToken(user);
            var expiration = DateTime.UtcNow.AddHours(1); // Token süresiyle eşleşmeli

            return new LoginResponseDTO
            {
                Token = token,
                Email = user.Email,
                UserId = user.Id,
                TokenExpiration = expiration
            };
        }
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("email", user.Email),
        new Claim("userId", user.Id.ToString())  
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
