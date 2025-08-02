using MediatR;
using WalletApp.Application.Feature.Auth.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.Auth.Handlers
{
    public class DeleteAccountRequestDTOHandler : IRequestHandler<DeleteUserAccountRequestDTO, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAccountRequestDTOHandler(
            IUserRepository userRepository,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<string>> Handle(DeleteUserAccountRequestDTO request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.CurrentUser(); 
            // muhtemelen int dönüyor
            var user = await _userRepository.GetAsync(u => u.Id == userId);
            if (user == null)
                return ServiceResponse<string>.Fail("Kullanıcı doğrulanamadı.");

            await _userRepository.DeleteAsync(user);

            return ServiceResponse<string>.Ok("Kullanıcı başarıyla silindi.");
        }
    }
}
