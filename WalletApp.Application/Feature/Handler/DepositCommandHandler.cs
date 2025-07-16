using MediatR;
using WalletApp.Application.Feature.Command;
using WalletApp.Application.Feature.DTO;
using WalletApp.Application.Services.Repositories.EntitysRepository;
using WalletApp.Domain.Base;
using WalletApp.Domain.Enums;
using WalletApp.Application.Feature.Queries;

namespace WalletApp.Application.Feature.Handler;

public class DepositCommandHandler : IRequestHandler<DepositCommand, ServiceResponse<TransactionResponseDTO>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;

    public DepositCommandHandler(IWalletRepository walletRepository, ITransactionRepository transactionRepository)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<ServiceResponse<TransactionResponseDTO>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        // Validation
        if (request.Amount <= 0)
            return ServiceResponse<TransactionResponseDTO>.Fail("Amount must be greater than zero.");

        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
        if (wallet == null)
            return ServiceResponse<TransactionResponseDTO>.Fail("Wallet not found.");

        if (wallet.UserId != request.RequestedBy)
            return ServiceResponse<TransactionResponseDTO>.Fail("You do not own this wallet.");

        // İşlem
        var transaction = new Transaction
        {
            WalletId = request.WalletId,
            Amount = request.Amount,
            Type = TransactionType.Deposit,
            Description = request.Description ?? "Deposit"
        };

        await _transactionRepository.AddAsync(transaction);

        var dto = new TransactionResponseDTO
        {
            WalletId = transaction.WalletId,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description
        };

        return ServiceResponse<TransactionResponseDTO>.Ok(dto, "Deposit successful.");
    }
}
