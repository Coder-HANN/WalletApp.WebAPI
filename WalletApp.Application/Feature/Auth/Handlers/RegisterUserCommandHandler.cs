using MediatR;
using Microsoft.AspNetCore.Identity;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;

public class RegisterUserCommandHandler : IRequestHandler<RegisterRequestDTO, ServiceResponse<RegisterResponseDTO>>
{
    private readonly IUserDetailRepository _userDetailRepository;
    private readonly WalletService _walletService;
    private readonly UserManager<AppUser> _userManager;

    public RegisterUserCommandHandler(
        IUserDetailRepository userDetailRepository,
        WalletService walletService,
        UserManager<AppUser> userManager)
    {
        _userDetailRepository = userDetailRepository;
        _walletService = walletService;
        _userManager = userManager;
    }

    public async Task<ServiceResponse<RegisterResponseDTO>> Handle(RegisterRequestDTO request, CancellationToken cancellationToken)
    {
        // Email zaten kayıtlı mı?
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return ServiceResponse<RegisterResponseDTO>.Fail("Bu e-posta zaten kayıtlı.");

        var user = new AppUser
        {
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            CreatedDate = DateTime.UtcNow,
            UserDetail = new UserDetail
            {
                Name = request.Name,
                Surname = request.Surname,
                Gender = request.Gender,
                BirthDay = request.BirthDay,
                Occupation = request.Occupation,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber
            }
        };

        try
        {
            // Kullanıcıyı UserManager ile oluştur
            var result = await _userManager.CreateAsync(user, request.PasswordHash);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ServiceResponse<RegisterResponseDTO>.Fail("Kayıt sırasında hata oluştu: " + errors);
            }

            // Kullanıcıya "User" rolü ata
            await _userManager.AddToRoleAsync(user, "User");

            // Wallet oluştur
            await _walletService.CreateWalletAsync(user.Id, "TL", cancellationToken);

            return ServiceResponse<RegisterResponseDTO>.Ok(new RegisterResponseDTO
            {
                Email = user.Email,
                Name = user.UserDetail.Name,
                Surname = user.UserDetail.Surname,
                Message = "Kayıt başarılı."
            }, "Kayıt başarılı.");
        }
        catch (Exception ex)
        {
            return ServiceResponse<RegisterResponseDTO>.Fail("Kayıt sırasında hata oluştu: " + ex.Message);
        }
    }
}
