using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

public class LoginUserCommandHandler : IRequestHandler<LoginUserRequestDTO, ServiceResponse<LoginUserResponseDTO>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly IConfiguration _configuration;

    public LoginUserCommandHandler(
        UserManager<AppUser> userManager,
        IPasswordHasher<AppUser> passwordHasher,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<LoginUserResponseDTO>> Handle(LoginUserRequestDTO request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return ServiceResponse<LoginUserResponseDTO>.Fail("Email veya şifre hatalı.");

        var passwordCheck = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (passwordCheck == PasswordVerificationResult.Failed)
            return ServiceResponse<LoginUserResponseDTO>.Fail("Email veya şifre hatalı.");

        var token = await GenerateJwtToken(user);
        var expiration = DateTime.UtcNow.AddHours(1);

        return ServiceResponse<LoginUserResponseDTO>.Ok(new LoginUserResponseDTO
        {
            Token = token,
            Email = user.Email,
            AppUserId = user.Id,
            TokenExpiration = expiration
        }, "Giriş başarılı.");
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

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

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
