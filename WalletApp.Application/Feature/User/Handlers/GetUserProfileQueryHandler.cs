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

            public GetUserProfileQueryHandler(IUserDetailRepository userDetailRepository, IHttpContextAccessor httpContextAccessor)
            {
                _userDetailRepository = userDetailRepository;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<ServiceResponse<UserProfileResponseDTO>> Handle(UserProfileRequestDTO request, CancellationToken cancellationToken)
            {
                // Token'dan AppUserId alma
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("AppUserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return ServiceResponse<UserProfileResponseDTO>.Fail("Kullanıcı doğrulanamadı");
                }

                // Kullanıcı detaylarını getir
                var userDetail = await _userDetailRepository.GetAsync(x => x.AppUserId == userId);

                if (userDetail == null)
                {
                    return ServiceResponse<UserProfileResponseDTO>.Fail("Profil bulunamadı");
                }

                // Bilgileri güncelle
                userDetail.Name = request.Name;
                userDetail.BirthDay = request.BirthDay;
                userDetail.Occupation = request.Occupation;
                userDetail.PhoneNumber = request.PhoneNumber;

                await _userDetailRepository.UpdateAsync(userDetail);
                await _userDetailRepository.SaveChangesAsync();

                // Response DTO oluştur
                var dto = new UserProfileResponseDTO
                {
                    Name = userDetail.Name,
                    BirthDay = request.BirthDay,
                    Occupation = userDetail.Occupation,
                    PhoneNumber = userDetail.PhoneNumber,
                };

                return ServiceResponse<UserProfileResponseDTO>.Ok(dto);
            }
        }

    
}
