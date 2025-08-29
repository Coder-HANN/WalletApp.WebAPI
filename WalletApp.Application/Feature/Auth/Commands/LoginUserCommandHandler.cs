using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.DTOs.Auth;
using WalletApp.Application.Feature.Auth.Commands;
using WalletApp.Application.Feature.Auth.Validators.Resource;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ServiceResponse<LoginUserResponseDTO>>
{
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    
    public LoginUserCommandHandler(
        IPasswordHasher<AppUser> passwordHasher,
        IConfiguration configuration,
        IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _userRepository = userRepository;
        
    }

    public async Task<ServiceResponse<LoginUserResponseDTO>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return ServiceResponse<LoginUserResponseDTO>.Fail(LoginResource.EmailorPasswordRequired);

        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return ServiceResponse<LoginUserResponseDTO>.Fail(LoginResource.InvalidEmailOrPassword);

        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verifyResult == PasswordVerificationResult.Failed)
            return ServiceResponse<LoginUserResponseDTO>.Fail(LoginResource.EmailorPasswordRequired);

        var token = await GenerateJwtToken(user);
        var expiration = DateTime.UtcNow.AddHours(1);

        return ServiceResponse<LoginUserResponseDTO>.Ok(new LoginUserResponseDTO
        {
            Token = token,
            Email = user.Email,
            TokenExpiration = expiration
        }, LoginResource.SuccessMessage);
    }

    private async Task<string> GenerateJwtToken(AppUser user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
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
