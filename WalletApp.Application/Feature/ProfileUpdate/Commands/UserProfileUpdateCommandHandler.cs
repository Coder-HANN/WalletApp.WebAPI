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
            var userId = _currentUserService.CurrentUser();
            if (userId == null)
                return ServiceResponse<UserProfileResponseDTO>.Fail("Kullanıcı bulunamadı");

            var userDetail = await _userDetailRepository.GetAsync(x => x.AppUserId == userId);
            if (userDetail == null)
                return ServiceResponse<UserProfileResponseDTO>.Fail("Profil bulunamadı");

            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user == null)
                return ServiceResponse<UserProfileResponseDTO>.Fail("Kullanıcı bilgisi bulunamadı");

            bool isModified = false;

            // Her alanı karşılaştır ve değiştiyse güncelle
            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != userDetail.Name)
            {
                userDetail.Name = request.Name;
                isModified = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Surname) && request.Surname != userDetail.Surname)
            {
                userDetail.Surname = request.Surname;
                isModified = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
                user.Email = request.Email;
                isModified = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Gender) && request.Gender != userDetail.Gender)
            {
                userDetail.Gender = request.Gender;
                isModified = true;
            }

            if (request.BirthDay != default && request.BirthDay != userDetail.BirthDay)
            {
                userDetail.BirthDay = request.BirthDay;
                isModified = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Occupation) && request.Occupation != userDetail.Occupation)
            {
                userDetail.Occupation = request.Occupation;
                isModified = true;
            }

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber) && request.PhoneNumber != userDetail.PhoneNumber)
            {
                userDetail.PhoneNumber = request.PhoneNumber;
                isModified = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Address) && request.Address != userDetail.Address)
            {
                userDetail.Address = request.Address;
                isModified = true;
            }

            if (!isModified)
            {
                return ServiceResponse<UserProfileResponseDTO>.Ok(new UserProfileResponseDTO
                {
                    Name = userDetail.Name,
                    Surname = userDetail.Surname,
                    Email = user.Email,
                    Gender = userDetail.Gender,
                    BirthDay = userDetail.BirthDay,
                    Occupation = userDetail.Occupation,
                    PhoneNumber = userDetail.PhoneNumber,
                    Address = userDetail.Address
                });
            }

            await _userDetailRepository.UpdateAsync(userDetail);
            await _userRepository.UpdateAsync(user);
            await _userDetailRepository.SaveChangesAsync();

            var dto = new UserProfileResponseDTO
            {
                Name = userDetail.Name,
                Surname = userDetail.Surname,
                Email = user.Email,
                Gender = userDetail.Gender,
                BirthDay = userDetail.BirthDay,
                Occupation = userDetail.Occupation,
                PhoneNumber = userDetail.PhoneNumber,
                Address = userDetail.Address,
            };

            return ServiceResponse<UserProfileResponseDTO>.Ok(dto);
        }

    }
}
