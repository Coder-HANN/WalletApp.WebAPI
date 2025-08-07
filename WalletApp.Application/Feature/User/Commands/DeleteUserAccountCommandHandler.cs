using MediatR;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.Feature.User.Commands;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.Auth.Handlers
{
    public class DeleteAccountRequestDTOHandler : IRequestHandler<DeleteUserAccountCommand, ServiceResponse<string>>
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

        public async Task<ServiceResponse<string>> Handle(DeleteUserAccountCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.CurrentUser();
            if (currentUserId == null)
                return ServiceResponse<string>.Fail("Kullanıcı doğrulanamdı");


            var user = await _userRepository.GetAsync(u => u.Id == currentUserId);

            if (user.Email != request.Email)
                return ServiceResponse<string>.Fail("Girilen email, giriş yapan kullanıcıya ait değil.");

            if (user.PasswordHash != request.PasswordHash)
                return ServiceResponse<string>.Fail("Girilen şifre hatalı.");

            var command = request.Command;
                
            if (!string.IsNullOrEmpty(command))
                    return ServiceResponse<string>.Fail("Lütfen hesabınızı neden kapatmak istediğinizi yazınız.");
            

            await _userRepository.DeleteAsync(user);

            return ServiceResponse<string>.Ok("Kullanıcı başarıyla silindi.");
        }
    }
}
