using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.DTOs.ProfileUpdate;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.ProfileUpdate.Commands
{
        public class UserProfileUpdateCommandHandler : IRequestHandler<UserProfileUpdateCommand, ServiceResponse<UserProfileResponseDTO>>
        {
            private readonly IUserDetailRepository _userDetailRepository;
            private readonly IUserRepository _userRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICurrentUserService _currentUserService;

            public UserProfileUpdateCommandHandler(
                IUserDetailRepository userDetailRepository, 
                IHttpContextAccessor httpContextAccessor,
                ICurrentUserService currentUserService,
                IUserRepository userRepository)
            {
                _userDetailRepository = userDetailRepository;
                _httpContextAccessor = httpContextAccessor;
                _currentUserService = currentUserService;
                _userRepository = userRepository;
        }

            public async Task<ServiceResponse<UserProfileResponseDTO>> Handle(UserProfileUpdateCommand request, CancellationToken cancellationToken)
            {
            // Token'dan AppUserId alma
            var userId = _currentUserService.CurrentUser();
            if (userId == null)
                return ServiceResponse<UserProfileResponseDTO>.Fail("Kullanıcı bulunamadı");

                // Kullanıcı detaylarını getir
                var userDetail = await _userDetailRepository.GetAsync(x => x.AppUserId == userId);
                
                if (userDetail == null)
                {
                    return ServiceResponse<UserProfileResponseDTO>.Fail("Profil bulunamadı");
                }
                var userEmail = await _userRepository.GetByEmailAsync(request.Email);


            // Bilgileri güncelle
            userDetail.Name = request.Name;
            userDetail.Surname = request.Surname;
            userEmail.Email = request.Email;
            userDetail.Gender = request.Gender;
            userDetail.BirthDay = request.BirthDay;
            userDetail.Occupation = request.Occupation;
            userDetail.PhoneNumber = request.PhoneNumber;
            userDetail.Address = request.Address;

            await _userDetailRepository.UpdateAsync(userDetail);
                await _userDetailRepository.SaveChangesAsync();

                // Response DTO oluştur
                var dto = new UserProfileResponseDTO
                {
                    Name = userDetail.Name,
                    Surname = userDetail.Surname,
                    Email = userEmail.Email,
                    Gender = userDetail.Gender,
                    BirthDay = request.BirthDay,
                    Occupation = userDetail.Occupation,
                    PhoneNumber = userDetail.PhoneNumber,
                    Address = userDetail.Address,
                };
            
                return ServiceResponse<UserProfileResponseDTO>.Ok(dto);
            }
        }
}
