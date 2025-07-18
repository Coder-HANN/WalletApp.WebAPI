using MediatR;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.User.Dtos;
using WalletApp.Application.Feature.User.Dtos.UserProfile;



public class GetUserProfileQueryHandler : IRequestHandler<UserProfileQueryResponseDTO, ServiceResponse<UserProfileResponseDTO>>
{
    private readonly IUserDetailRepository _userDetailRepository;

    public GetUserProfileQueryHandler(IUserDetailRepository userDetailRepository)
    {
        _userDetailRepository = userDetailRepository;
    }

    public async Task<ServiceResponse<UserProfileResponseDTO>> Handle(UserProfileQueryResponseDTO request, CancellationToken cancellationToken)
    {
        var detail = await _userDetailRepository.GetAsync(x => x.AppUserId == request.AppUserId); // Corrected method call to use GetAsync with a predicate  
        if (detail == null)
            return ServiceResponse<UserProfileResponseDTO>.Fail("Profil bulunamadı");

        return ServiceResponse<UserProfileResponseDTO>.Ok(new UserProfileResponseDTO
        {
            Name = detail.Name,
            PhoneNumber = detail.PhoneNumber,
            BirthDay = detail.BirthDay,
            Occupation = detail.Occupation
        });
    }
}
