using MediatR;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.DTOs.Payment;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.Payment.Handler
{
    public class PaymentCommandHandler : IRequestHandler<PaymentCommand, ServiceResponse<PaymentResponseDTO>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUserService;

        public PaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;

        }

         public async Task<ServiceResponse<PaymentResponseDTO>> Handle(PaymentCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.CurrentUser();
            if (currentUserId == null)
                return ServiceResponse<PaymentResponseDTO>.Fail("Kullanıcı doğrulanamdı");
            // ➤ Kontrol: Institution boş mu?
            if (string.IsNullOrWhiteSpace(request.Institution))
                return ServiceResponse<PaymentResponseDTO>.Fail("Kuruluş adı boş olamaz.");

            // ➤ Kontrol: Amount negatif mi?
            if (request.Amount <= 0)
                return ServiceResponse<PaymentResponseDTO>.Fail("Ödeme miktarı pozitif olmalıdır.");

            // ➤ Cüzdan kontrolü
            var wallet = await _walletRepository.GetAsync(w => w.Id == request.AppWalletId);
            if (wallet == null)
                return ServiceResponse<PaymentResponseDTO>.Fail("Cüzdan bulunamadı.");

            // ➤ Bakiye kontrolü
            if (wallet.TotalBalance < request.Amount)
            {
                return ServiceResponse<PaymentResponseDTO>.Fail("Yetersiz bakiye.");
            }
            // ➤ Transaction oluştur
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                Amount = -request.Amount,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow
            };
            await _transactionRepository.AddAsync(transaction);
            

            // ➤ Payment oluştur
            var payment = new AppPayment
            {
                Id = Guid.NewGuid(),
                TransactionId = transaction.Id,
                Amount = request.Amount,
                Institution = request.Institution,
                CreatedDate = DateTime.UtcNow
            };
            await _paymentRepository.AddAsync(payment);
           

            // ➤ Bakiye güncelle
            wallet.TotalBalance -= request.Amount;
            await _walletRepository.UpdateAsync(wallet);
            await _walletRepository.SaveChangesAsync();

            // ➤ Response
            var response = new PaymentResponseDTO
            {
                AppPaymentId = payment.Id,
                Institution = request.Institution,
                Amount = request.Amount,
                PaymentDate = DateTime.UtcNow
            };

            return ServiceResponse<PaymentResponseDTO>.Ok(response, "Ödeme başarıyla gerçekleşti");
        }
    }
}
