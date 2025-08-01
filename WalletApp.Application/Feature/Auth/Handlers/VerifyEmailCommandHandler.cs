using MediatR;
using Microsoft.Extensions.Caching.Memory;
using WalletApp.Application.Feature.Auth.Dtos;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Auth.Handlers
{
    public class VerifyEmailRequestDTOHandler : IRequestHandler<VerifyEmailRequestDTO, ServiceResponse<string>>
    {
        private readonly IMemoryCache _cache;

        public VerifyEmailRequestDTOHandler(
            IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<ServiceResponse<string>> Handle(VerifyEmailRequestDTO request, CancellationToken cancellationToken)
        {
            var cacheKey = $"email-verification-{request.Email}";

            if (!_cache.TryGetValue(cacheKey, out string? storedCode))
            {
                return Task.FromResult(ServiceResponse<string>.Fail("Kod süresi dolmuş veya hiç gönderilmemiş."));
            }

            if (storedCode != request.VerificationCode)
            {
                return Task.FromResult(ServiceResponse<string>.Fail("Doğrulama kodu hatalı."));
            }

            _cache.Remove(cacheKey); // Kod doğru -> sil

            return Task.FromResult(ServiceResponse<string>.Ok("E-posta doğrulandı."));
        }
    }
}
