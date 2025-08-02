using MediatR;
using Microsoft.AspNetCore.Identity;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;

public class RegisterUserCommandHandler : IRequestHandler<RegisterRequestDTO, ServiceResponse<RegisterResponseDTO>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserDetailRepository _userDetailRepository;
    private readonly WalletService _walletService;
    private readonly IPasswordHasher<AppUser> _passwordHasher;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUserDetailRepository userDetailRepository,
        WalletService walletService,
        IPasswordHasher<AppUser> passwordHasher)
    {
        _userRepository = userRepository;
        _userDetailRepository = userDetailRepository;
        _walletService = walletService;
        _passwordHasher = passwordHasher;
    }

    public async Task<ServiceResponse<RegisterResponseDTO>> Handle(RegisterRequestDTO request, CancellationToken cancellationToken)
    {
        if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
            return ServiceResponse<RegisterResponseDTO>.Fail("Bu e-posta zaten kayıtlı.");

        var user = new AppUser
        {
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(null, request.PasswordHash),
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
            _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();
            await _walletService.CreateWalletAsync(user.Id, "TL", cancellationToken);
        }
        catch (Exception ex)
        {
            return ServiceResponse<RegisterResponseDTO>.Fail("Kayıt sırasında hata oluştu: " + ex.Message);
        }

        return ServiceResponse<RegisterResponseDTO>.Ok(new RegisterResponseDTO
        {
            Email = user.Email,
            Name = user.UserDetail.Name,
            Surname = user.UserDetail.Surname,
            Message = "Kayıt başarılı."
        }, "Kayıt başarılı.");
    }
}
