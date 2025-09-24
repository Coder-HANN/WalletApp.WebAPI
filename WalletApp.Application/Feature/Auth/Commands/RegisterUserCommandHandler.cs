using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.MailServices;
using WalletApp.Application.DTOs.Auth;
using WalletApp.Application.Feature.Auth.Commands;
using WalletApp.Application.Feature.Auth.Validators.Resource;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;

public class RegisterUserCommandHandler : IRequestHandler<RegisterCommand, ServiceResponse<RegisterResponseDTO>>
{
    private readonly IUserDetailRepository _userDetailRepository;
    private readonly IUserRepository _userRepository;
    private readonly WalletService _walletService;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasher<AppUser> _passwordHasher;


    public RegisterUserCommandHandler(
        IUserDetailRepository userDetailRepository,
        WalletService walletService,
        IUserRepository userRepository,
        IEmailService emailService,
        IPasswordHasher<AppUser> passwordHasher)
    {
        _userDetailRepository = userDetailRepository;
        _walletService = walletService;
        _emailService = emailService;
        _userRepository = userRepository;
        _passwordHasher = (IPasswordHasher<AppUser>?)passwordHasher;
    }

    public async Task<ServiceResponse<RegisterResponseDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Email zaten kayıtlı mı?
        if(await _userDetailRepository.ExistsAsync(request.Email))
        {
            return ServiceResponse<RegisterResponseDTO>.Fail(RegisterResource.RegisteredEmail);
        }
        if (!request.Email.Contains("@"))
            return ServiceResponse<RegisterResponseDTO>.Fail(RegisterResource.InvalidEmailFormat);

        var user = new AppUser
        {
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            CreatedDate = DateTime.UtcNow,
            Role = UserRole.User
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.PasswordHash);

       
        await _userRepository.AddUserAsync(user);  

        var userDetail = new UserDetail
        {
            AppUserId = user.Id, 
            Name = request.Name,
            Surname = request.Surname,
            Gender = request.Gender,
            BirthDay = request.BirthDay,
            Occupation = request.Occupation,
            Address = string.IsNullOrEmpty(request.Address) ? string.Empty : request.Address,
            PhoneNumber = request.PhoneNumber
        };

        await _userDetailRepository.AddAsync(userDetail);

        try
        {    
            // Wallet oluştur
            await _walletService.CreateWalletAsync( "TL", cancellationToken);

            return ServiceResponse<RegisterResponseDTO>.Ok(new RegisterResponseDTO
            {
                Email = user.Email,
                Name = user.UserDetail.Name,
                Surname = user.UserDetail.Surname,
                Message = RegisterResource.SuccessMessage
            },  RegisterResource.SuccessMessage);
        }
        catch (Exception ex)
        {
            return ServiceResponse<RegisterResponseDTO>.Fail(RegisterResource.ErrorMessage + ex.Message);
        }
    }
}
