using MediatR;
using WalletApp.Application.Feature.Payment.DTO;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;

namespace WalletApp.Application.Feature.Payment.Handler
{
    public class PaymentCommandHandler : IRequestHandler<PaymentRequestDTO, ServiceResponse<PaymentResponseDTO>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public PaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository)
        {
            _paymentRepository = paymentRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }


        public Task<ServiceResponse<PaymentRequestDTO>> Handle(PaymentRequestDTO request, CancellationToken cancellationToken)
        {

        }
    }
}
