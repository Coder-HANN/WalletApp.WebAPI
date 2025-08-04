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
    private readonly IEmailService _emailService;


    public RegisterUserCommandHandler(
        IUserDetailRepository userDetailRepository,
        WalletService walletService,
        IEmailService emailService)
    {
        _userDetailRepository = userDetailRepository;
        _walletService = walletService;
        _emailService = emailService;
    }

    public async Task<ServiceResponse<RegisterResponseDTO>> Handle(RegisterRequestDTO request, CancellationToken cancellationToken)
    {
        // Email zaten kayıtlı mı?
        if(await _userDetailRepository.ExistsAsync(request.Email))
        {
            return ServiceResponse<RegisterResponseDTO>.Fail("Bu e-posta adresi zaten kayıtlı.");
        }
        if (!request.Email.Contains("@"))
            return ServiceResponse<RegisterResponseDTO>.Fail("Geçersiz email formatı.");


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
            await _emailService.SendAsync(
                request.Email,
                "Kayıt Başarılı",
                $"Merhaba {request.Name}, kayıt işleminiz başarıyla tamamlandı. Hoş geldiniz! Kodunuz: 123456");
            // Kullanıcı detaylarını ekle

            await _userDetailRepository.AddAsync(user.UserDetail);
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
