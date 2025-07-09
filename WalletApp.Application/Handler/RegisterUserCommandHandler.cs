using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Application.DTO;
using WalletApp.Application.DTO.CommentDTO;
using WalletApp.Application.Handler.RegisterUserCommandHandler;
using WalletApp.Domain.Base;



    



namespace WalletApp.Application.Handler.RegisterUserCommandHandler
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponseDTO>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IWalletRepository walletRepository, IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
        {
            _walletRepository = walletRepository;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<RegisterResponseDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RegisterDTO;

            var emailExists = await _userRepository.EmailExistsAsync(dto.Email, cancellationToken);


            if (emailExists)
            {
                throw new Exception("Bu e-posta zaten kayıtlı.");
            }

            var user = new User

            {
                 
                Email = dto.Email
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            
            UserDetail userDetail = new UserDetail
            {
                Name = dto.Name,
                BirthDay = dto.BirthDay,
                PhoneNumber = dto.PhoneNumber,
                Occupation = dto.Occupation
            };

            _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();

            return new RegisterResponseDTO
            {
                Name = userDetail.Name,
                Email = user.Email,
                Message = "Kayıt başarılı"
            };
        }
    }
}
