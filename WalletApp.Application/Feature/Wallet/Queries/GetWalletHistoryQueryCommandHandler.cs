using MediatR;
using Microsoft.AspNetCore.Http;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.Common.Pagination;
using WalletApp.Application.DTOs.Wallet;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Domain.Entities;


namespace WalletApp.Application.Feature.Wallet.Queries
{
    public class GetWalletHistoryQueryCommandHandler : PagedSearchQueryHandler<GetUserWalletsHistoryQuery, ServiceResponse<IEnumerable<TransactionResponseDTO>>> 
    {
        private readonly WalletService _walletService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEntityRepository<AppWallet> _entityRepository;

        public GetWalletHistoryQueryCommandHandler(
            WalletService walletService,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService,
            IEntityRepository<AppWallet> entityRepository)
        {
            _walletService = walletService;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
            _entityRepository = entityRepository;
        }

        public override async Task<ServiceResponse<IEnumerable<TransactionResponseDTO>>> Handle(GetUserWalletsHistoryQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _walletService.GetWalletTransactionHistoryAsync(request.WalletId);

            if (!transactions.Any())
                return ServiceResponse<IEnumerable<TransactionResponseDTO>>.Fail("İşlem geçmişi bulunamadı.");

            var dtoList = transactions.Select(t => new TransactionResponseDTO
            {
                Id = t.Id,
                WalletId = t.WalletId,
                Amount = t.Amount,
                Type = t.Type,
                Description = t.Description,
                CreatedDate = t.CreatedDate
            });

            var result = await _entityRepository.GetPagedResult<TransactionResponseDTO>(dtoList,
               pageSize: request.PageSize,
               pageIndex: request.Page,
               ordering: shr => shr.OrderByDescending(_ => _.Id),
               cancellationToken: cancellationToken);

            return HandleResult<TransactionResponseDTO>(result);
        }
    }
}