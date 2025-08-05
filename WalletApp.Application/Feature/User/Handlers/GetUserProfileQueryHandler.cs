using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Feature.User.Dtos.UserProfile;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.User.Handlers
{
        public class GetUserProfileQueryHandler : IRequestHandler<UserProfileRequestDTO, ServiceResponse<UserProfileResponseDTO>>
        {
            private readonly IUserDetailRepository _userDetailRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICurrentUserService _currentUserService;

            public GetUserProfileQueryHandler(
                IUserDetailRepository userDetailRepository, 
                IHttpContextAccessor httpContextAccessor,
                ICurrentUserService currentUserService)
            {
                _userDetailRepository = userDetailRepository;
                _httpContextAccessor = httpContextAccessor;
                _currentUserService = currentUserService;
            }

            public async Task<ServiceResponse<UserProfileResponseDTO>> Handle(UserProfileRequestDTO request, CancellationToken cancellationToken)
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

                // Bilgileri güncelle
                userDetail.Name = request.Name;
                userDetail.Surname = request.Surname;
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
